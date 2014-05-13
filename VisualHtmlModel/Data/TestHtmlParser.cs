using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualHtmlModel.Data
{
    public class TestHtmlParser : IHtmlParser
    {
        private HtmlDocument _doc = new HtmlDocument();

        public List<HtmlElement> GetRootElements()
        {
            List<HtmlElement> result = new List<HtmlElement>();


            for (HtmlNode node = _doc.DocumentNode; node != null; node = node.NextSibling)
            {
                result.Add(new HtmlElement
                {
                    Name = node.Name,
                    InnerText = node.InnerText,
                    ChildElements = GetChildElements(node),
                    Attributes = GetAttributes(node)
                });
            }
            return result;
        }

        public async Task<int> Load(string url)
        {
            using (StreamReader sr = File.OpenText(@"C:\Users\Bryan\Documents\Visual Studio 2013\Projects\VisualHtml\VisualHtmlModel\Data\test-page.htm"))
            {
                var content = await sr.ReadToEndAsync();
                _doc.LoadHtml(content);
            }
            return 0;
        }

        private Dictionary<string, string> GetAttributes(HtmlNode node)
        {
            if (node.HasAttributes)
            {
                var result = new Dictionary<string, string>();
                foreach (var attribute in node.Attributes)
                {
                    if (!result.ContainsKey(attribute.Name))
                        result.Add(attribute.Name, attribute.Value);
                }
                return result;
            }
            return null;
        }

        private List<HtmlElement> GetChildElements(HtmlNode startNode)
        {
            if (startNode.HasChildNodes)
            {
                var result = new List<HtmlElement>();
                foreach (HtmlNode node in startNode.ChildNodes)
                {
                    result.Add(new HtmlElement
                    {
                        Name = node.Name,
                        InnerText = node.InnerText,
                        ChildElements = GetChildElements(node),
                        Attributes = GetAttributes(node)
                    });
                }
                return result;
            }
            return null;
        }
        private async Task LoadHtml(string url)
        {
            HtmlWeb webGet = new HtmlWeb();
            _doc = await Task.Run<HtmlDocument>(() => webGet.Load(url));
        }
    }
}
