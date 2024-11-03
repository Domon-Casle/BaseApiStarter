using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CoreUtilities.DI
{
    public static class DependencyInjectionExtentionMethods
    {
        public static void BuildDependencyInjectionFromAttribute(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var diClasses = assembly.GetTypes().Where(c => c.IsDefined(typeof(InjectDependencyAttribute), true));
            foreach (var diClass in diClasses)
            {
                var diAttribute = diClass.GetCustomAttribute<InjectDependencyAttribute>(true);
                if (diAttribute != null)
                {
                    switch (diAttribute.Scope)
                    {
                        case DIScope.Scoped:
                            serviceCollection.AddScoped(diAttribute.InterfaceType, diClass);
                            break;

                        case DIScope.Singleton:
                            serviceCollection.AddSingleton(diAttribute.InterfaceType, diClass);
                            break;

                        case DIScope.Transiant:
                            serviceCollection.AddTransient(diAttribute.InterfaceType, diClass);
                            break;

                        default:
                            throw new ArgumentException("Invalid Scope defined for DI");
                    }
                }
            }
        }
    }
}
