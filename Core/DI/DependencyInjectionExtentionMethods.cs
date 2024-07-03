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
                            serviceCollection.AddScoped(diClass.GetType(), diAttribute.InterfaceType);
                            break;

                        case DIScope.Singleton:
                            serviceCollection.AddTransient(diClass.GetType(), diAttribute.InterfaceType);
                            break;

                        case DIScope.Transiant:
                            serviceCollection.AddSingleton(diClass.GetType(), diAttribute.InterfaceType);
                            break;

                        default:
                            throw new ArgumentException("Invalid Scope defined for DI");
                    }
                }
            }
        }
    }
}
