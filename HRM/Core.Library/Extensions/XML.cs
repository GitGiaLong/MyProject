using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Linq;

namespace Core.Library.Extensions
{
    public class XML
    {
        private static XDocument xDoc = new XDocument();

        private string _FILE_PATH = "WebApi";
        public string FILE_PATH { get { return _FILE_PATH; } set { _FILE_PATH = value ; } }
        

        public string ReadXML(string nameXML, string atr)
        {
            //if(xDoc.Descendants(nameXML).Count() <= 0)
            //{
            //    WriteXML("Connect", "ServerName", "ROYALDRAGON\\ROYALDRAGON_2012");
            //}
            foreach (var item in xDoc.Descendants(nameXML))
            {
                // src will be null if the attribute is missing
                //string src = (string)item.Attribute(atr);
                return (string)item.Attribute(atr) ?? "";
                //item.SetAttributeValue("serverName", src + "with-changes");
            }
            return "";
        }

        public void WriteXML(string ElementName, string AttributeName, string AttributeValue)
        {
            xDoc = XDocument.Load(FILE_PATH);
            xDoc.Root.Add(new XElement(ElementName,
                new XAttribute(AttributeName, AttributeValue)
            ));
            xDoc.Save(FILE_PATH);
        }
        public void createAndLoadXML()
        {
            // Method to create XML file based on name entered by user
            ////string tempPath = ";
            string configFileName = FILE_PATH;
            //string configPath = tempPath + configFileName + ".config";
            string configPath = configFileName + ".config";
            // Create XDocument
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf8", "yes")
                ,new XElement("Connect",
                        new XAttribute("ServerName", "ROYALDRAGON\\ROYALDRAGON_2012"),
                        new XAttribute("DatabaseName", "HRM"),
                        new XAttribute("Username", ""),
                        new XAttribute("Password", "")
                        )
                );
            document.Save(configPath);
            //configCreateLabel.Visible = true;
            document = XDocument.Load(configPath);
        }
        public void SerializeSettingToFile<T>(string path,T value)
        {
            string jsonString;
            JsonSerializerSettings jsSetting = new JsonSerializerSettings();
            jsSetting.TypeNameHandling = TypeNameHandling.Auto;
            jsSetting.Formatting = Formatting.Indented;
            jsonString = JsonConvert.SerializeObject(value, jsSetting);
            File.WriteAllText(path, jsonString, System.Text.Encoding.UTF8);
        }
    }
}
