using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace AdaptiveProgrammingModel
{
    public class XMLSerializer : IDLLSerializer
    {
        public void Serialize(AssemblyMetadata assemblyMetadata, Stream stream)
        {
            using (stream)
            {
                XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true });
                DataContractSerializer serializer = new DataContractSerializer(typeof(AssemblyMetadata), null, Int32.MaxValue, true, true, null);
                
                serializer.WriteObject(writer, assemblyMetadata);
                writer.Flush();
            }
        }

        public AssemblyMetadata Deserialize(Stream stream)
        {
            using (stream)
            {
                XmlReader reader = XmlReader.Create(stream);
                DataContractSerializer serializer = new DataContractSerializer(typeof(AssemblyMetadata), null, Int32.MaxValue, true, true, null);

                AssemblyMetadata data = (AssemblyMetadata)serializer.ReadObject(reader);
                return data;
            }
        }
    }
}