using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using AdaptiveProgrammingData.Bases;


namespace AdaptiveProgrammingData
{
    public class XMLSerializer : IDLLSerializer
    {
        IEnumerable<Type> KnownTypes = new Type[]
        {
            typeof(AssemblyXML), typeof(NamespaceXML), typeof(TypeXML),
            typeof(MethodXML), typeof(PropertyXML),
            typeof(ParameterXML)
        };
        public void Serialize(AssemblyBase assembly)
        {
            using (Stream stream = new FileStream("../../../SerializationFile/assembly.xml", FileMode.Create,
                FileAccess.Write))
            {
                DataContractSerializer dataContractSerializer = new DataContractSerializer(assembly.GetType(),KnownTypes);
                dataContractSerializer.WriteObject(stream, assembly);
                TraceAP.InfoLog("Serialization done", "XMLSerializer");
            }
        }

        public AssemblyBase Deserialize()
        {
            Stream stream = new FileStream("../../../SerializationFile/assembly.xml", FileMode.Open, FileAccess.Read);
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(AssemblyXML),KnownTypes);
            XmlDictionaryReader xmlReader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
            AssemblyBase deserializedData = (AssemblyBase)dataContractSerializer.ReadObject(xmlReader);
            TraceAP.InfoLog("Serialization done", "XMLSerializer");
            return deserializedData;
        }
    }
}