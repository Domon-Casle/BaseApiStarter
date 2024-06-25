using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace CoreUtilities.Logger
{
    public class IgnorableSerializerContractResolver : DefaultContractResolver
    {
        protected readonly IEnumerable<Type> _attributeTypesToIgnore;

        public IgnorableSerializerContractResolver(IEnumerable<Type> attributeTypesToIgnore)
        {
            _attributeTypesToIgnore = attributeTypesToIgnore;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            var propertyAttributes = property.AttributeProvider?.GetAttributes(true);
            if (propertyAttributes != null)
            {
                foreach (var pa in propertyAttributes)
                {
                    if (_attributeTypesToIgnore.Any(x => x == pa.GetType()))
                    {
                        property.Ignored = true;
                    }
                }
            }

            return property;
        }
    }
}
