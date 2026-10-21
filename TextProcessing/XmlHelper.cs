// using System.IO;
using System.Xml.Serialization;

namespace lab3.TextProcessing;

static class XmlHelper
{
    public static void ExportToXml(Text text, string filePath)
    {
        XmlSerializer serializer = new(typeof(Text));
        using StreamWriter writer = new(filePath);
        serializer.Serialize(writer, text);
    }

    public static Text ImportFromXml(string filePath)
    {
        XmlSerializer serializer = new(typeof(Text));
        using StreamReader reader = new(filePath);
        return (Text)serializer.Deserialize(reader);
    }
}
