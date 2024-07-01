using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using Core.WPF.Interactivities.Enums;

namespace Core.WPF.Interactivities
{
    internal sealed class Serializer
    {
        private Serializer() { }

        public class Data
        {
            public static readonly int CurrentSchemaVersion = 2;

            // If the SchemaVersion attribute is missing, we assume it's v1.
            public static readonly int DefaultSchemaVersion = 1;

            public static readonly int MinValidSchemaVersion = 1;

            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
                Justification = "The use of visible nested classes for serialization seems reasonable.")]
            public class RuntimeOptionsData
            {
                public bool HideNavigation { get; set; }
                public bool HideAnnotationAndInk { get; set; }
                public bool DisableInking { get; set; }
                public bool HideDesignTimeAnnotations { get; set; }
                public bool ShowDesignTimeAnnotationsAtStart { get; set; }
            }

            
            public class ViewStateData
            {
                public double Zoom { get; set; }
                public Point? Center { get; set; }
            }

            
            public class Screen
            {
                public ScreenType Type { get; set; }

                public string ClassName { get; set; }
                public string DisplayName { get; set; }
                public string FileName { get; set; }

            
                public List<Annotation> Annotations { get; set; }

                public Point Position { get; set; }
                public int? VisualTag { get; set; }

                public Screen()
                {
                    Annotations = new List<Annotation>();
                }
            }

            
            public class VisualTag
            {
                public string Name { get; set; }
                public string Color { get; set; }
            }

            [XmlAttribute]
            public int SchemaVersion { get; set; }

            public Guid SketchFlowGuid { get; set; }
            public string StartScreen { get; set; }

            public List<Screen> Screens { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
                Justification = "These collections are not part of a 'public' API, and it's just too handy to be able to replace the whole list.")]
            [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists",
                Justification = "These generic lists are fine in their context. There is no need to listen to individual changes and they are more performant.")]
            public string SharePointDocumentLibrary { get; set; }
            public string SharePointProjectName { get; set; }
            public int PrototypeRevision { get; set; }
            public string BrandingText { get; set; }
            public RuntimeOptionsData RuntimeOptions { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
                Justification = "These collections are not part of a 'public' API, and it's just too handy to be able to replace the whole list.")]
            [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists",
                Justification = "These generic lists are fine in their context. There is no need to listen to individual changes and they are more performant.")]
            public List<VisualTag> VisualTags { get; set; }
            public ViewStateData ViewState { get; set; }

            public Data()
            {
                // When deserializing, if the SchemaVersion is not included in the file, we default to v1.
                SchemaVersion = DefaultSchemaVersion;

                RuntimeOptions = new RuntimeOptionsData();
                ViewState = new ViewStateData();
                VisualTags = new List<VisualTag>();
                Screens = new List<Screen>();
            }
        }

        public static Color HexStringToColor(string value)
        {
            if (value.Length != 8)
            {
                throw new InvalidOperationException("Serializer.HexStringToColor requires input of a 8-character hexadecimal string, but received '" + value + "'.");
            }

            byte a = byte.Parse(value.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            byte r = byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            byte g = byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            byte b = byte.Parse(value.Substring(6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);

            return Color.FromArgb(a, r, g, b);
        }

        public static string ColorToHexString(Color color)
        {
            string a = color.A.ToString("X2", CultureInfo.InvariantCulture);
            string r = color.R.ToString("X2", CultureInfo.InvariantCulture);
            string g = color.G.ToString("X2", CultureInfo.InvariantCulture);
            string b = color.B.ToString("X2", CultureInfo.InvariantCulture);
            return a + r + g + b;
        }

        public static void Serialize(Data data, Stream stream)
        {
            data.SchemaVersion = Data.CurrentSchemaVersion;

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = true
            };

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Data));
                serializer.Serialize(writer, data);
            }
        }

        public static Data Deserialize(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return Deserialize(stream);
            }
        }

        public static Data Deserialize(Stream stream)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Data));
                return serializer.Deserialize(stream) as Data;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public static int? GetSchemaVersion(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element &&
                            StringComparer.InvariantCultureIgnoreCase.Equals(reader.LocalName, "Data"))
                        {
                            reader.MoveToAttribute("SchemaVersion");
                            break;
                        }
                    }

                    int? schemaVersion = null;
                    if (!reader.EOF)
                    {
                        int value;
                        if (int.TryParse(reader.Value, out value))
                        {
                            schemaVersion = value;
                        }
                    }

                    return schemaVersion;
                }
            }
        }
    }
}
