using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AdaptiveProgrammingData;
using AdaptiveProgrammingData.Bases;
using MethodBase = System.Reflection.MethodBase;

namespace AdaptiveProgrammingModel
{
    [DataContract(IsReference = true)]
    public class MethodMetadata : AdaptiveProgrammingData.Bases.MethodBase
    {
        public override string Name { get; set; }
        public override List<TypeBase> GenericArguments { get; set; }
        public override Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }
        public override TypeBase ReturnType { get; set; }
        public override bool Extension { get; set; }
        public override List<ParameterBase> Parameters { get; set; }

        private MethodMetadata(MethodBase method)
        {
            this.Name = method.Name;
            this.GenericArguments = !method.IsGenericMethodDefinition ? null : TypeMetadata.EmitGenericArguments(method.GetGenericArguments());
            this.ReturnType = EmitReturnType(method);
            this.Parameters = EmitParameters(method.GetParameters());
            this.Modifiers = EmitModifiers(method);
            this.Extension = EmitExtension(method);
        }

        public MethodMetadata(AdaptiveProgrammingData.Bases.MethodBase methodBase)
        {
            Name = methodBase.Name;
            Extension = methodBase.Extension;

            if (methodBase.Modifiers != null)
            {
                Modifiers = methodBase.Modifiers;
            }

            GetReurnType(methodBase);
            FillGenericArguments(methodBase);
            FillParameters(methodBase);

        }

        private void FillParameters(AdaptiveProgrammingData.Bases.MethodBase methodBase)
        {
            Parameters = new List<ParameterBase>();
            if (methodBase.Parameters != null)
            {
                foreach (ParameterBase parameter in methodBase.Parameters)
                {
                    Parameters.Add(new ParameterMetadata(parameter));
                }
            }
        }

        private void FillGenericArguments(AdaptiveProgrammingData.Bases.MethodBase methodBase)
        {
            if (methodBase.GenericArguments != null)
            {
                foreach (TypeBase arg in methodBase.GenericArguments)
                {
                    if (BaseDictionary.typeDictionary.ContainsKey(arg.TypeName))
                    {
                        GenericArguments.Add(BaseDictionary.typeDictionary[arg.TypeName]);
                    }
                    else
                    {
                        GenericArguments.Add(new TypeMetadata(arg));
                    }
                }
            }
        }

        private void GetReurnType(AdaptiveProgrammingData.Bases.MethodBase methodBase)
        {
            if (methodBase.ReturnType != null)
            {
                if (BaseDictionary.typeDictionary.ContainsKey(methodBase.ReturnType.TypeName))
                {
                    ReturnType = BaseDictionary.typeDictionary[methodBase.ReturnType.TypeName];
                }
                else
                {
                    ReturnType = new TypeMetadata(methodBase.ReturnType);
                }
            }
        }

        private static List<ParameterBase> EmitParameters(IEnumerable<ParameterInfo> parms)
        {
            List<ParameterMetadata> parameterMetadatas = (from parm in parms
                select new ParameterMetadata(parm.Name, TypeMetadata.EmitReference(parm.ParameterType))).ToList();
            List<ParameterBase> parameterBases = new List<ParameterBase>();
            foreach (ParameterMetadata parameter in parameterMetadatas)
            {
                parameterBases.Add(parameter);
            }
            return parameterBases;
        }
        private static TypeBase EmitReturnType(MethodBase method)
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
        public static List<AdaptiveProgrammingData.Bases.MethodBase> EmitMethods(IEnumerable<MethodBase> methods)
        {
            List<MethodMetadata> methodMetadatas = (from MethodBase _currentMethod in methods
                where _currentMethod.GetVisible()
                select new MethodMetadata(_currentMethod)).ToList();
            List<AdaptiveProgrammingData.Bases.MethodBase> methodBases = new List<AdaptiveProgrammingData.Bases.MethodBase>();
            foreach (MethodMetadata method in methodMetadatas)
            {
                methodBases.Add(method);
            }

            return methodBases;
        }
    }
}