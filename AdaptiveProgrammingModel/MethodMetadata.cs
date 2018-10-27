using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AdaptiveProgrammingModel
{
    public class MethodMetadata
    {
        private string name;
        private IEnumerable<TypeMetadata> genericArguments;
        private Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> modifiers;
        private TypeMetadata returnType;
        private bool extension;
        private IEnumerable<ParameterMetadata> parameters;

        private MethodMetadata(MethodBase method)
        {
            this.name = method.Name;
            this.genericArguments = !method.IsGenericMethodDefinition ? null : TypeMetadata.EmitGenericArguments(method.GetGenericArguments());
            this.returnType = EmitReturnType(method);
            this.parameters = EmitParameters(method.GetParameters());
            this.modifiers = EmitModifiers(method);
            this.extension = EmitExtension(method);
        }

        private static IEnumerable<ParameterMetadata> EmitParameters(IEnumerable<ParameterInfo> parms)
        {
            return from parm in parms
                select new ParameterMetadata(parm.Name, TypeMetadata.EmitReference(parm.ParameterType));
        }
        private static TypeMetadata EmitReturnType(MethodBase method)
        {
            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return null;
            return TypeMetadata.EmitReference(methodInfo.ReturnType);
        }
        private static bool EmitExtension(MethodBase method)
        {
            return method.IsDefined(typeof(ExtensionAttribute), true);
        }
        private static Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> EmitModifiers(MethodBase method)
        {
            AccessLevel _access = AccessLevel.IsPrivate;
            if (method.IsPublic)
                _access = AccessLevel.IsPublic;
            else if (method.IsFamily)
                _access = AccessLevel.IsProtected;
            else if (method.IsFamilyAndAssembly)
                _access = AccessLevel.IsProtectedInternal;
            AbstractEnum _abstract = AbstractEnum.NotAbstract;
            if (method.IsAbstract)
                _abstract = AbstractEnum.Abstract;
            StaticEnum _static = StaticEnum.NotStatic;
            if (method.IsStatic)
                _static = StaticEnum.Static;
            VirtualEnum _virtual = VirtualEnum.NotVirtual;
            if (method.IsVirtual)
                _virtual = VirtualEnum.Virtual;
            return new Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum>(_access, _abstract, _static, _virtual);
        }
        public static IEnumerable<MethodMetadata> EmitMethods(IEnumerable<MethodBase> methods)
        {
            return from MethodBase _currentMethod in methods
                where _currentMethod.GetVisible()
                select new MethodMetadata(_currentMethod);
        }
    }
}