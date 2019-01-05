using System;
using System.Reflection;
using AdaptiveProgrammingData;
using AdaptiveProgrammingData.Bases;
using AdaptiveProgrammingModel;

namespace AdaptiveProgrammingViewModel.Mapper
{
    public static class AssemblyMapper
    {
        public static AssemblyBase GetSerializableAssembly(AssemblyMetadata assemblyMetadata, Type type)
        {
            object serializableAssembly = Activator.CreateInstance(type);
            PropertyInfo assemblyName = type.GetProperty("AssemblyName");
            PropertyInfo namespaces = type.GetProperty("Namespaces");
            assemblyName?.SetValue(serializableAssembly,assemblyMetadata.AssemblyName);
            return (AssemblyBase)serializableAssembly;
        }
    }
}