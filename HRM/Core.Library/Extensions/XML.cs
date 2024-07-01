using System.Xml.Linq;

namespace Core.Library.Extensions
{
    public static class XML
    {
        
        public static string GetAttributesXML(XDocument doc, string nameXML, string atr)
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
