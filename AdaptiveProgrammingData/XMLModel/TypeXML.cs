using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    [DataContract(IsReference = true)]
    public class TypeXML : TypeBase
    {
        [DataMember] public override bool IsSupplemented { get; set; }
        [DataMember] public override string TypeName { get; set; }
        [DataMember] public override string NamespaceName { get; set; }
        [DataMember] public override TypeBase BaseType { get; set; }
        [DataMember] public override List<TypeBase> GenericArguments { get; set; }
        [DataMember] public override Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }
        [DataMember] public override TypeKind TypeKind { get; set; }
        [DataMember] public override List<TypeBase> ImplementedInterfaces { get; set; }
        [DataMember] public override List<TypeBase> NestedTypes { get; set; }
        [DataMember] public override List<PropertyBase> Properties { get; set; }
        [DataMember] public override TypeBase DeclaringType { get; set; }
        [DataMember] public override List<MethodBase> Methods { get; set; }
        [DataMember] public override List<MethodBase> Constructors { get; set; }
    }
}