using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AwesomeWallpaper.Utils
{
    public static class SerializeUtils
    {
        public static string Serialize<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var xmlWriter = new XmlTextWriter(stringWriter))
            {
                xmlWriter.Formatting = Formatting.Indented;
                serializer.Serialize(xmlWriter, obj);
            }
            return stringWriter.ToString();
        }

        public static T Deserialize<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new StringReader(xml));
        }
    }
}
