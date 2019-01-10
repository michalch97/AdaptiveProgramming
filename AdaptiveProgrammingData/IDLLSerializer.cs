using System;
using System.IO;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    public interface IDLLSerializer
    {
        void Serialize(AssemblyBase assembly);
        AssemblyBase Deserialize();
    }
}