using System;
using System.IO;

namespace AdaptiveProgrammingData
{
    public interface IDLLSerializer
    {
        void Serialize<T>(T data);
        T Deserialize<T>();
    }
}