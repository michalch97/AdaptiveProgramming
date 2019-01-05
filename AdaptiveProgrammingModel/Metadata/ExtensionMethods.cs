using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdaptiveProgrammingModel
{
    public static class ExtensionMethods
    {
        public static bool GetVisible(this Type type)
        {
            return type.IsPublic || type.IsNestedPublic || type.IsNestedFamily || type.IsNestedFamANDAssem;
        }
        public static bool GetVisible(this MethodBase method)
        {
            return method != null && (method.IsPublic || method.IsFamily || method.IsFamilyAndAssembly);
        }
        public static string GetNamespace(this Type type)
        {
            string ns = type.Namespace;
            return ns != null ? ns : string.Empty;
        }
    }
}