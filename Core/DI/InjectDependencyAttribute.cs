namespace CoreUtilities.DI
{
    public enum DIScope
    {
        Transiant,
        Scoped,
        Singleton
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class InjectDependencyAttribute(Type interfaceType, DIScope scope = DIScope.Scoped) : Attribute
    {
        public Type InterfaceType { get; set; } = interfaceType;
        public DIScope Scope { get; set; } = scope;
    }
}
