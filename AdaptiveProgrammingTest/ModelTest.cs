using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using AdaptiveProgrammingModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdaptiveProgrammingTest
{
    [TestClass]
    public class ModelTest
    {
        private AssemblyMetadata assemblyMetadata;
        private NamespaceMetadata namespaceMetadata;

        [TestInitialize]
        public void ModelTestInitialize()
        {
            assemblyMetadata = AssemblyLoader.LoadAssembly("../../../TestFile/TPA.ApplicationArchitecture.dll");
            namespaceMetadata = assemblyMetadata.Namespaces[0];
        }
        [TestMethod]
        public void AssemblyLoaderSuccessTest()
        {
            AssemblyMetadata assemblyMetadata = AssemblyLoader.LoadAssembly("../../../TestFile/TPA.ApplicationArchitecture.dll");
            Assert.IsTrue(assemblyMetadata.AssemblyName == "TPA.ApplicationArchitecture.dll");
        }
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void AssemblyLoaderExceptionTest()
        {
            AssemblyMetadata assemblyMetadata = AssemblyLoader.LoadAssembly("../../../TestFile/TPA.Application.dll");
        }
        [TestMethod]
        public void ExtensionMethodGetVisibleType()
        {
            foreach (Type type in namespaceMetadata.Tp)
            {
                Assert.AreEqual(type.GetVisible(),false);
            }
        }
        [TestMethod]
        public void ExtensionMethodGetNamespaceTest()
        {
            foreach (Type type in namespaceMetadata.Tp)
            {
                Assert.AreEqual(type.GetNamespace(), namespaceMetadata.NamespaceName);
            }
        }
        [TestMethod]
        public void ExtensionMethodGetVisibleMethodTest()
        {
            Type type = namespaceMetadata.Tp[0];
            var methods = type.GetMethods().ToList();
            foreach (MethodBase methodBase in namespaceMetadata.Tp[0].GetMethods())
            {
                Assert.AreEqual(methodBase.GetVisible(), true);
            }
        }
        [TestMethod]
        public void JSONSerializationTest()
        {
            IDLLSerializer jsonSerializer = new JSONSerializer();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                jsonSerializer.Serialize(assemblyMetadata, memoryStream);
                memoryStream.Position = 0;
                StreamReader reader = new StreamReader(memoryStream);
                string serializedAssembly = reader.ReadToEnd();
                StringAssert.Contains(serializedAssembly, "TPA.ApplicationArchitecture.dll");
                StringAssert.Contains(serializedAssembly, "TPA.ApplicationArchitecture.BusinessLogic");
                StringAssert.Contains(serializedAssembly, "TPA.ApplicationArchitecture.Data");
                StringAssert.Contains(serializedAssembly, "TPA.ApplicationArchitecture.Presentation");
                StringAssert.Contains(serializedAssembly, "Model");
                StringAssert.Contains(serializedAssembly, "Linq2SQL");
                StringAssert.Contains(serializedAssembly, "Linq_2_SQL");
                StringAssert.Contains(serializedAssembly, "get_Linq_2_SQL");
                StringAssert.Contains(serializedAssembly, "set_Linq_2_SQL");
                StringAssert.Contains(serializedAssembly, "ToString");
                StringAssert.Contains(serializedAssembly, "Equals");
                StringAssert.Contains(serializedAssembly, "GetHashCode");
                StringAssert.Contains(serializedAssembly, "GetType");
                StringAssert.Contains(serializedAssembly, "ServiceA");
                StringAssert.Contains(serializedAssembly, "Service_A");
                StringAssert.Contains(serializedAssembly, "ServiceB");
                StringAssert.Contains(serializedAssembly, "Service_B");
                StringAssert.Contains(serializedAssembly, "ServiceC");
                StringAssert.Contains(serializedAssembly, "Service_C");
                StringAssert.Contains(serializedAssembly, "ViewModel");
                StringAssert.Contains(serializedAssembly, "Connect");
                StringAssert.Contains(serializedAssembly, "View");
            }
        }
        [TestMethod]
        public void JSONDeserializationTest()
        {
            IDLLSerializer jsonSerializer = new JSONSerializer();
            using (FileStream fileStream = new FileStream("../../../TestFile/assembly.json",FileMode.Open))
            {
                AssemblyMetadata assembly = jsonSerializer.Deserialize(fileStream);
                Assert.AreEqual(assembly.AssemblyName,"TPA.ApplicationArchitecture.dll");
                Assert.AreEqual(assembly.Namespaces[0].NamespaceName, "TPA.ApplicationArchitecture.BusinessLogic");
                Assert.AreEqual(assembly.Namespaces[1].NamespaceName, "TPA.ApplicationArchitecture.Data");
                Assert.AreEqual(assembly.Namespaces[2].NamespaceName, "TPA.ApplicationArchitecture.Presentation");
            }
        }
    }
}
