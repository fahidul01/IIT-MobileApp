using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IIT.Web.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllTypes<T>(this IServiceCollection services, Assembly assembly)
        {

            var types = assembly.GetTypes()
                                .Where(myType => myType.IsClass &&
                                      !myType.IsAbstract &&
                                      myType.IsSubclassOf(typeof(T)));

            foreach (var type in types)
            {
                services.AddScoped(type);
            }
        }
    }
}
