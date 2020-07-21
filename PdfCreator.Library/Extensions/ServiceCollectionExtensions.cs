using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PdfCreator.Library.Commands;
using System;
using System.Linq;
using System.Reflection;

namespace PdfCreator.Library.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllTypes<T>(this IServiceCollection services, Assembly[] assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient, string[] excludedTypes = null)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
            {
                if (excludedTypes == null || !excludedTypes.Contains(type.Name))
                {
                    services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
                }
            }
        }
    }
}
