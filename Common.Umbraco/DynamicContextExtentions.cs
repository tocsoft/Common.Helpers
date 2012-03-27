using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Common.Helpers;
using umbraco.cms.businesslogic.macro;
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
                var dic = properties.ToDictionary();

                foreach (var d in dic)
                {
                    macroModel.Properties.Add(new MacroPropertyModel(d.Key, d.Value.ToString()));
                }
            }

            var control = macro.renderMacro(new System.Collections.Hashtable(), ctx.Node.Id);

            return new HtmlString("");//control.RenderControlToString());
        }
    }
}