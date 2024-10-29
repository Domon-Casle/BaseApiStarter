using CoreUtilities;
using CoreUtilities.DI;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CoreDomain
{
    public static class CoreDomainDependencyInjectionSetup
    {
        public static void DepenedencySetup(IServiceCollection services)
        {
            CoreUtilityDependencyInjectionSetup.DepenedencySetup(services);

            var myAssembly = Assembly.GetAssembly(typeof(TableCache));
            if (myAssembly == null)
            {
                throw new Exception("Could not find domain assembly");
            }

            services.BuildDependencyInjectionFromAttribute(myAssembly);
        }
    }
}
