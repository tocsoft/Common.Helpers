using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using Tocsoft.Common.Helpers;
using Tocsoft.Common.Helpers.Web;

namespace Tocsoft.Common.Umbraco
{
    public class ControlHtmlString : IHtmlString
    {
        Lazy<string> _controlRenderer;

        public ControlHtmlString(Control ctl)
        {
            _controlRenderer = new Lazy<string>(() => ctl.RenderControlToString());
        }

        public string Html {
            get {
                return _controlRenderer.Value;
            }
        }

        public override string ToString()
        {
            return Html;
        }

        public string ToHtmlString()
        {
            return Html;
        }
    }
}