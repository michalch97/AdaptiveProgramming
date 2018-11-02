using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AdaptiveProgrammingModel
{
    public class AssemblyLoader
    {
        public static AssemblyMetadata LoadAssembly(string assemblyFile)
        {
            FileInfo assemblyFileInfo = new FileInfo(assemblyFile);
            if (!assemblyFileInfo.Exists)
            {
                throw new FileNotFoundException(nameof(assemblyFile));
            }

            Assembly assembly = Assembly.LoadFrom(assemblyFileInfo.FullName);

            return new AssemblyMetadata(assembly);
        }
    }
}