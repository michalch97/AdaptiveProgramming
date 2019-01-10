using System.Collections.Generic;
using AdaptiveProgrammingData.Bases;

namespace AdaptiveProgrammingData
{
    public static class BaseDictionary
    {
        public static Dictionary<string, TypeBase> typeDictionary = new Dictionary<string, TypeBase>();
        public static Dictionary<string, PropertyBase> propertyDictionary = new Dictionary<string, PropertyBase>();
    }
}