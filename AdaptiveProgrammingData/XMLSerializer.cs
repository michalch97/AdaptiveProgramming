using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using AdaptiveProgrammingData.Bases;
using AdaptiveProgrammingMEF;
using AdaptiveProgrammingTrace;


namespace AdaptiveProgrammingData
{
    [Export(typeof(IDLLSerializer))]
    public class XMLSerializer : IDLLSerializer
    {
        [Import(typeof(ITrace))]
        public ITrace Trace { get; set; }
        IEnumerable<Type> KnownTypes = new Type[]
        {
            typeof(AssemblyXML), typeof(NamespaceXML), typeof(TypeXML),
            typeof(MethodXML), typeof(PropertyXML),
            typeof(ParameterXML)
        };
        public void Serialize(AssemblyBase assembly)
        {
            MEF.Compose(this);
            using (Stream stream = new FileStream("../../../SerializationFile/assembly.xml", FileMode.Create,
                FileAccess.Write))
            {
                DataContractSerializer dataContractSerializer = new DataContractSerializer(assembly.GetType(),KnownTypes);
                dataContractSerializer.WriteObject(stream, assembly);
                Trace.InfoLog("Serialization done", "XMLSerializer");
            }
        }

        public AssemblyBase Deserialize()
        {
            MEF.Compose(this);
            Stream stream = new FileStream("../../../SerializationFile/assembly.xml", FileMode.Open, FileAccess.Read);
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(AssemblyXML),KnownTypes);
            XmlDictionaryReader xmlReader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
            AssemblyBase deserializedData = (AssemblyBase)dataContractSerializer.ReadObject(xmlReader);
            Trace.InfoLog("Serialization done", "XMLSerializer");
            return deserializedData;
        }
    }
}