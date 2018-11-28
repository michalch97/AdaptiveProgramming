using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace AdaptiveProgrammingModel
{
    public class XMLSerializer: IDLLSerializer
    {
        public void Serialize(AssemblyMetadata assemblyMetadata, Stream stream)
        {
            DataContractSerializer dataContractSerializer = new DataContractSerializer(assemblyMetadata.GetType());
            dataContractSerializer.WriteObject(stream,assemblyMetadata);
            TraceAP.InfoLog("Serialization done", "XMLSerializer");
        }

        public AssemblyMetadata Deserialize(Stream stream)
        {
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(AssemblyMetadata));
            XmlDictionaryReader xmlReader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
            AssemblyMetadata assemblyMetadata = (AssemblyMetadata)dataContractSerializer.ReadObject(xmlReader);
            TraceAP.InfoLog("Serialization done", "XMLSerializer");
            return assemblyMetadata;
        }
    }
}