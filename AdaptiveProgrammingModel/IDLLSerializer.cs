using System.IO;

namespace AdaptiveProgrammingModel
{
    public interface IDLLSerializer
    {
        void Serialize(AssemblyMetadata assemblyMetadata, Stream stream);
        AssemblyMetadata Deserialize(Stream stream);
    }
}