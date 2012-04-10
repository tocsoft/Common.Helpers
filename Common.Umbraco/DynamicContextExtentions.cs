using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Common.Helpers;
using Common.Helpers.Web;
using umbraco.cms.businesslogic.macro;
using umbraco.interfaces;
using umbraco.MacroEngines;

namespace Common.Umbraco
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

        public static string ImageUrl(this DynamicNodeContext ctx, DynamicNode node, string alias, string cropProperty, string cropName)
        {
            var mediaProp = node.GetProperty(alias);
            string url = null;
            if (mediaProp != null && !string.IsNullOrWhiteSpace(mediaProp.Value))
            {
                DynamicMedia media = ctx.Library.MediaById(mediaProp.Value);
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
            }

            return url ?? "";
        }

        public static string ImageUrl(this DynamicNodeContext ctx, string alias, string cropProperty, string cropName)
        {
            return ImageUrl(ctx, (DynamicNode)ctx.Model, alias, cropProperty, cropName);
        }
    }
}