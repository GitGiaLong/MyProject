using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Core.Libraries.Extensions;

namespace Core.Libraries.Helpers
{
    public class XmlSerializerHelper
    {
        public static void Serialize<T>(T obj, string fullPath) where T : class, new()
        {
            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var writer = new StreamWriter(fullPath))
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
            }
        }

        public static T Deserialize<T>(string fullPath, string xmlns = null) where T : class, new()
        {
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(string.Format("\"{0}\" file does not exists.", fullPath));
            }

            using (var reader = new StreamReader(fullPath))
            {
                XmlSerializer serializer = null;
                if (xmlns == null)
                {
                    serializer = new XmlSerializer(typeof(T));
                }
                else
                {
                    serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlns));
                }

                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="xmlWriterSettings"> The xml writer settings. </param>
        /// <typeparam name="T"> The object of the type T. </typeparam>
        /// <returns> The value in <see cref="string"/> that correspond the xml. </returns>
        /// <exception cref="ArgumentException"> The value cannot be null. </exception>
        public static string Serialize<T>(T obj, XmlWriterSettings xmlWriterSettings = null)
        {
            if (obj == null)
            {
                throw new ArgumentException("value");
            }

            var serializer = new XmlSerializer(typeof(T));

            var settings = xmlWriterSettings ?? new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(),
                Indent = true,
                OmitXmlDeclaration = false
            };

            using (var textWriter = new ExtentedStringWriter(new StringBuilder(), Encoding.UTF8))
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    serializer.Serialize(xmlWriter, obj, ns);
                }

                return textWriter.ToString();
            }
        }

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="xml"> The xml. </param>
        /// <param name="xmlReaderSettings"> The xml Reader Settings. </param>
        /// <typeparam name="T"> The object of the type T. </typeparam>
        /// <returns> The object of the type <see cref="T"/>. </returns>
        /// <exception cref="ArgumentException"> The xml value cannot be null. </exception>
        public static T Deserialize<T>(string xml, XmlReaderSettings xmlReaderSettings = null, string xmlns = null)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("xml");
            }

            XmlSerializer serializer = null;
            if (xmlns == null)
            {
                serializer = new XmlSerializer(typeof(T));
            }
            else
            {
                serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlns));
            }

            var settings = xmlReaderSettings ?? new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreWhitespace = true,
                IgnoreComments = true
            };


            /// No settings need modifying here
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                using (var xmlReader = XmlReader.Create(ms, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }
    }
}
