namespace CoreUtilities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DIAttribute(Type interfaceType) : Attribute
    {
        public Type InterfaceType { get; set; } = interfaceType;
    }
}
