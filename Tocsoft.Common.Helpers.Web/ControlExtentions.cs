using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Tocsoft.Common.Helpers.Web
{
    public static class ControlExtentions
    {
        public static string RenderControlToString(this Control cont)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            var hw = new HtmlTextWriter(sw);
            cont.RenderControl(hw);

            return sb.ToString();
        }
    }
}