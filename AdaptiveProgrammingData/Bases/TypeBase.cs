using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdaptiveProgrammingData.Bases
{
    [DataContract(IsReference = true)]
    public abstract class TypeBase
    {
        [DataMember]
        public virtual bool IsSupplemented { get; set; }
        [DataMember]
        public virtual string TypeName { get; set; }
        [DataMember]
        public virtual string NamespaceName { get; set; }
        [DataMember]
        public virtual TypeBase BaseType { get; set; }
        [DataMember]
        public virtual List<TypeBase> GenericArguments { get; set; }
        [DataMember]
        public virtual Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }
        [DataMember]
        public virtual TypeKind TypeKind { get; set; }
        [DataMember]
        public virtual List<TypeBase> ImplementedInterfaces { get; set; }
        [DataMember]
        public virtual List<TypeBase> NestedTypes { get; set; }
        [DataMember]
        public virtual List<PropertyBase> Properties { get; set; }
        [DataMember]
        public virtual TypeBase DeclaringType { get; set; }
        [DataMember]
        public virtual List<MethodBase> Methods { get; set; }
        [DataMember]
        public virtual List<MethodBase> Constructors { get; set; }

    }
}