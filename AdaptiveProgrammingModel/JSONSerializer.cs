using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    public class JSONSerializer : IDLLSerializer
    {
        public void Serialize(AssemblyMetadata assemblyMetadata, Stream stream)
        {
            using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
            {
                string json = JsonConvert.SerializeObject(assemblyMetadata, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                    {
                       PreserveReferencesHandling = PreserveReferencesHandling.Objects//,
                       //ConstructorHandling = ConstructorHandling.Default
                        //ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                    }
                    );
                streamWriter.Write(json);
                streamWriter.Flush();
            }
        }

        public AssemblyMetadata Deserialize(Stream stream)
        {
            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
            AssemblyMetadata assemblyMetadata = JsonConvert.DeserializeObject<AssemblyMetadata>(streamReader.ReadToEnd(), new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects//,
                    //ConstructorHandling = ConstructorHandling.Default
                    //ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                }
            );

            return assemblyMetadata;
        }
    }
}