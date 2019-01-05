using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AdaptiveProgrammingData;
using Newtonsoft.Json;

namespace AdaptiveProgrammingModel
{
    [DataContract(IsReference = true)]
    public class MethodMetadata
    {
        [DataMember]
        private string name;
        [DataMember]
        private List<TypeMetadata> genericArguments;
        [DataMember]
        private Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> modifiers;
        [DataMember]
        private TypeMetadata returnType;
        [DataMember]
        private bool extension;
        [DataMember]
        private List<ParameterMetadata> parameters;
        public string Name
        {
            get { return this.name; }
            private set { this.name = value; }
        }
        public List<TypeMetadata> GenericArguments
        {
            get { return this.genericArguments; }
            private set { this.genericArguments = value; }
        }
        public Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers
        {
            get { return this.modifiers; }
            private set { this.modifiers = value; }
        }
        public TypeMetadata ReturnType
        {
            get { return this.returnType; }
            private set { this.returnType = value; }
        }
        public bool Extension
        {
            get { return this.extension; }
            private set { this.extension = value; }
        }
        public List<ParameterMetadata> Parameters
        {
            get { return this.parameters; }
            private set { this.parameters = value; }
        }

        private MethodMetadata(MethodBase method)
        {
            this.Name = method.Name;
            this.GenericArguments = !method.IsGenericMethodDefinition ? null : TypeMetadata.EmitGenericArguments(method.GetGenericArguments());
            this.ReturnType = EmitReturnType(method);
            this.Parameters = EmitParameters(method.GetParameters());
            this.Modifiers = EmitModifiers(method);
            this.Extension = EmitExtension(method);
        }

        private static List<ParameterMetadata> EmitParameters(IEnumerable<ParameterInfo> parms)
        {
            return (from parm in parms
                    select new ParameterMetadata(parm.Name, TypeMetadata.EmitReference(parm.ParameterType))).ToList();
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
        public static List<MethodMetadata> EmitMethods(IEnumerable<MethodBase> methods)
        {
            return (from MethodBase _currentMethod in methods
                    where _currentMethod.GetVisible()
                    select new MethodMetadata(_currentMethod)).ToList();
        }
    }
}