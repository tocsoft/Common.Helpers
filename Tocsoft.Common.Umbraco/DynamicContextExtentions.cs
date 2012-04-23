using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Tocsoft.Common.Helpers;
using Tocsoft.Common.Helpers.Web;
using umbraco.cms.businesslogic.macro;
using umbraco.interfaces;
using umbraco.MacroEngines;

namespace Tocsoft.Common.Umbraco
{
    public static class DynamicNodeContextExtentions
    {
        public static IHtmlString RenderMacro(this DynamicNodeContext ctx, string alias, params object[] properties)
        {
            var macro = umbraco.macro.GetMacro(alias);
            var macroModel = macro.Model;

            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    var dic = prop.ToDictionary();

                    foreach (var d in dic)
                    {
                        macroModel.Properties.Add(new MacroPropertyModel(d.Key, d.Value.ToString()));
                    }
                }
            }

            var control = macro.renderMacro(new System.Collections.Hashtable(), ctx.Node.Id);

            return new HtmlString(control.RenderControlToString());
        }

        private static string ImageUrlFromMediaItem(DynamicNodeContext ctx, int mediaId, string cropProperty, string cropName)
        {
            string url = null;
            DynamicMedia media = ctx.Library.MediaById(mediaId);
            if (media != null)
            {
                if (media.HasProperty(cropProperty))
                {
                    dynamic d = new DynamicXml(media.GetPropertyValue(cropProperty)).Find("@name", cropName);
                    url = (string)d.url;
                }

                if (string.IsNullOrWhiteSpace(url) && media.HasValue("umbracoFile"))
                {
                    url = media.GetPropertyValue("umbracoFile");
                }
            }
            return url ?? "";
        }

        private static string ImageUrlFromXml(DynamicNodeContext ctx, string xml , string cropProperty, string cropName)
        {
            string url = null;
            DynamicXml media = new DynamicXml(xml);
            if (media != null)
            {
                var crop = media.Descendants(x => x.Name == cropProperty).FirstOrDefault();
                if (crop != null)
                {
                    dynamic c = crop.Find("@name", cropName);
                    url = (string)c.url;
                }

                if (string.IsNullOrWhiteSpace(url))
                {
                    var f = media.Descendants(x => x.Name == "umbracoFile").FirstOrDefault();
                    if (f != null)
                        url = f.InnerText;
                }
            }
            return url ?? "";
        }

        public static string ImageUrl(this DynamicNodeContext ctx, DynamicNode node, string alias, string cropProperty, string cropName)
        {
            var mediaProp = node.GetProperty(alias);
            string url = null;
            if (mediaProp != null && !string.IsNullOrWhiteSpace(mediaProp.Value))
            {
                int mediaId = 0;
                if (int.TryParse(mediaProp.Value, out mediaId))
                    url = ImageUrlFromMediaItem(ctx, mediaId, cropProperty, cropName);
                else
                    url = ImageUrlFromXml(ctx, mediaProp.Value, cropProperty, cropName);
            }

            return url ?? "";
        }

        public static string ImageUrl(this DynamicNodeContext ctx, string alias, string cropProperty, string cropName)
        {
            return ImageUrl(ctx, ctx.Current, alias, cropProperty, cropName);
        }
    }
}