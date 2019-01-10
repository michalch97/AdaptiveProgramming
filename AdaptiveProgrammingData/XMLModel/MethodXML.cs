using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    [DataContract(IsReference = true)]
    public class MethodXML : MethodBase
    {
        [DataMember]
        public override string Name { get; set; }
        [DataMember]
        public override List<TypeBase> GenericArguments { get; set; }
        [DataMember]
        public override Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }
        [DataMember]
        public override TypeBase ReturnType { get; set; }
        [DataMember]
        public override bool Extension { get; set; }
        [DataMember]
        public override List<ParameterBase> Parameters { get; set; }

        public MethodXML(MethodBase methodBase)
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

        private void FillParameters(MethodBase methodBase)
        {
            Parameters = new List<ParameterBase>();
            if (methodBase.Parameters != null)
            {
                foreach (ParameterBase parameter in methodBase.Parameters)
                {
                    Parameters.Add(new ParameterXML(parameter));
                }
            }
        }

        private void FillGenericArguments(MethodBase methodBase)
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
                        GenericArguments.Add(new TypeXML(arg));
                    }
                }
            }
        }

        private void GetReurnType(MethodBase methodBase)
        {
            if (methodBase.ReturnType != null)
            {
                if (BaseDictionary.typeDictionary.ContainsKey(methodBase.ReturnType.TypeName))
                {
                    ReturnType = BaseDictionary.typeDictionary[methodBase.ReturnType.TypeName];
                }
                else
                {
                    ReturnType = new TypeXML(methodBase.ReturnType);
                }
            }
        }
    }
}