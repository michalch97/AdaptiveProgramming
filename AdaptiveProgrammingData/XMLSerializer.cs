using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;


namespace AdaptiveProgrammingData
{
    public class XMLSerializer : IDLLSerializer
    {
        public void Serialize<T>(T data)
        {
            using (Stream stream = new FileStream("../../../SerializationFile/assembly.xml", FileMode.Create,
                FileAccess.Write))
            {
                DataContractSerializer dataContractSerializer = new DataContractSerializer(data.GetType());
                dataContractSerializer.WriteObject(stream, data);
                TraceAP.InfoLog("Serialization done", "XMLSerializer");
            }
        }

        public T Deserialize<T>()
        {
            Stream stream = new FileStream("../../../SerializationFile/assembly.xml", FileMode.Open, FileAccess.Read);
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
            XmlDictionaryReader xmlReader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
            T deserializedData = (T)dataContractSerializer.ReadObject(xmlReader);
            TraceAP.InfoLog("Serialization done", "XMLSerializer");
            return deserializedData;
        }
    }
}