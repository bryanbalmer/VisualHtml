using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualHtmlModel
{
    public class HtmlElement
    {
        public string Name { get; set; }
        public List<HtmlElement> ChildElements { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public string InnerText { get; set; }
    }
}
