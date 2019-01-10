using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Schema;
using AdaptiveProgrammingMEF;
using AdaptiveProgrammingTrace;

namespace AdaptiveProgrammingModel
{
    public class AssemblyLoader
    {
        [Import(typeof(ITrace))]
        public ITrace Trace { get; set; }
        public static ObjectIDGenerator idGenerator = new ObjectIDGenerator();
        public static Dictionary<long, TypeMetadata> loadedTypes = new Dictionary<long, TypeMetadata>();
        public AssemblyMetadata LoadAssembly(string assemblyFile)
        {
            MEF.Compose(this);
            FileInfo assemblyFileInfo = new FileInfo(assemblyFile);
            if (!assemblyFileInfo.Exists)
            {
                Trace.ErrorLog("File not found", "AssemblyLoader");
                throw new FileNotFoundException(nameof(assemblyFile));
            }

            Assembly assembly = Assembly.LoadFrom(assemblyFileInfo.FullName);
            Trace.InfoLog("Assembly loaded", "AssemblyLoader");
            return new AssemblyMetadata(assembly);
        }
    }
}