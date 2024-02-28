using System.Xml.Linq;

namespace GSMF.Application.Local.Extensions
{
    public class XMLExtension
    {
        XDocument doc;
        public XMLExtension(string path) { doc = XDocument.Load(path); }
        public string GetAttributesXML(string nameXML, string atr)
        {
            foreach (var item in doc.Descendants(nameXML))
            {
                // src will be null if the attribute is missing
                //string src = (string)item.Attribute(atr);
                return (string)item.Attribute(atr) ?? "";
                //item.SetAttributeValue("serverName", src + "with-changes");
            }
            return "";
        }
    }
}
