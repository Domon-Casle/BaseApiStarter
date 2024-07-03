using CoreUtilities.DI;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CoreUtilities
{
    public static class CoreUtilityDependencyInjectionSetup
    {
        public static void CoreUtilityDISetup(IServiceCollection services)
        {
            var myAssembly = Assembly.GetAssembly(typeof(Require));
            if (myAssembly == null)
            {
                throw new Exception("Could not find domain assembly");
            }

            services.BuildDependencyInjectionFromAttribute(myAssembly);
        }
    }
}
