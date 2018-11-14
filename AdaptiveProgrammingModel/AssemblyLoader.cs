using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingModel
{
    public class AssemblyLoader
    {
        public static ObjectIDGenerator idGenerator = new ObjectIDGenerator();
        public static Dictionary<long, TypeMetadata> loadedTypes = new Dictionary<long, TypeMetadata>();
        public static AssemblyMetadata LoadAssembly(string assemblyFile)
        {
            FileInfo assemblyFileInfo = new FileInfo(assemblyFile);
            if (!assemblyFileInfo.Exists)
            {
                TraceAP.ErrorLog("File not found", "AssemblyLoader");
                throw new FileNotFoundException(nameof(assemblyFile));
            }

            Assembly assembly = Assembly.LoadFrom(assemblyFileInfo.FullName);
            TraceAP.InfoLog("Assembly loaded", "AssemblyLoader");
            return new AssemblyMetadata(assembly);
        }
    }
}