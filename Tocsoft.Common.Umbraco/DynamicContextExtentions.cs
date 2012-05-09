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
                macroModel.Properties.Clear();
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

        private static string ImageUrlFromXml(DynamicNodeContext ctx, DynamicXml media, string cropProperty, string cropName)
        {
            string url = null;
            if (media != null)
            {
                var crop = new DynamicXml(media.DescendantsOrSelf(x => x.Name == cropProperty).FirstOrDefault().ToXml());
                try
                {
                    if (crop != null)
                    {
                        dynamic c = crop.Find("@name", cropName);
                        url = (string)c.url;
                    }
                }
                catch { }

                if (string.IsNullOrWhiteSpace(url))
                {
                    var f = media.Descendants(x => x.Name == "umbracoFile").FirstOrDefault();
                    if (f != null)
                        url = f.InnerText;
                }
            }
            return url ?? "";
        }

        public static IEnumerable<string> ImageUrls(this DynamicNodeContext ctx, DynamicNode node, string alias, string cropProperty, string cropName)
        {
            var mediaProp = node.GetProperty(alias);

            if (mediaProp != null && !string.IsNullOrWhiteSpace(mediaProp.Value))
            {
                if (mediaProp.Value.Contains('<'))
                {
                    foreach (var m in new DynamicXml(mediaProp.Value).OfType<DynamicXml>())
                    {
                        var url = ImageUrlFromXml(ctx, m, cropProperty, cropName);
                        if (!url.IsNullOrWhiteSpace())
                            yield return url;
                    }
                }
                else
                {
                    //we look like a list ofr ids
                    foreach (var val in mediaProp.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        int mediaId = 0;
                        if (int.TryParse(val, out mediaId))
                        {
                            var url = ImageUrlFromMediaItem(ctx, mediaId, cropProperty, cropName);
                            if (!url.IsNullOrWhiteSpace())
                                yield return url;
                        }
                    }
                    //we look like xml
                }
            }
        }

        public static string ImageUrl(this DynamicNodeContext ctx, DynamicNode node, string alias, string cropProperty, string cropName)
        {
            return ImageUrls(ctx, node, alias, cropProperty, cropName).FirstOrDefault();
        }

        public static IEnumerable<string> ImageUrls(this DynamicNodeContext ctx, string alias, string cropProperty, string cropName)
        {
            return ImageUrls(ctx, ctx.Current, alias, cropProperty, cropName);
        }

        public static string ImageUrl(this DynamicNodeContext ctx, string alias, string cropProperty, string cropName)
        {
            return ImageUrl(ctx, ctx.Current, alias, cropProperty, cropName);
        }
    }
}