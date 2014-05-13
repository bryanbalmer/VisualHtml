using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VisualHtmlModel.Data
{
    public class HtmlParser : IHtmlParser
    {
        private HtmlDocument _doc = new HtmlDocument();

        public List<HtmlElement> GetRootElements()
        {
            List<HtmlElement> result = new List<HtmlElement>();

            for (HtmlNode rootNode = _doc.DocumentNode; rootNode != null; rootNode = rootNode.NextSibling)
            {
                result.Add(new HtmlElement
                    {
                        Name = _doc.DocumentNode.SelectSingleNode("//title").InnerText,
                        InnerText = rootNode.InnerText,
                        ChildElements = GetChildElements(rootNode),
                        Attributes = GetAttributes(rootNode)
                    });
            }
            return result;
        }

        public async Task<int> Load(string url)
        {

            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await LoadHtml(url);
                    return 0;
                }
                return 1;
            }
            catch
            {
                return 1;
            }
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
                    if (node.NodeType == HtmlNodeType.Element)
                    { 
                        result.Add(new HtmlElement
                        {
                            Name = node.Name,
                            InnerText = node.InnerText,
                            ChildElements = GetChildElements(node),
                            Attributes = GetAttributes(node)
                        });
                    }
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
