using Microsoft.Extensions.DependencyInjection;
using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Extensions;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Configuration
{
    public class AppInitialisation
    {
        public static T GetRequiredService<T>()
        {
            return GetRequiredService<T>(new[] { "" });
        }

        public static T GetRequiredService<T>(string[] args)
        {
            var serviceProvider = BuildServiceProvider(args);
            return serviceProvider.GetRequiredService<T>();
        }

        static public ServiceProvider BuildServiceProvider(string[] args)
        {
            var serviceProvider = GetServiceCollection().BuildServiceProvider();
            var appConfiguration = serviceProvider.GetRequiredService<IAppConfiguration>();
            appConfiguration.Load(args);

            return serviceProvider;
        }

        static public IServiceCollection GetServiceCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IAppConfiguration, AppConfiguration>();
            serviceCollection.AddSingleton<IPdfCreator, PdfCreator>();
            serviceCollection.AddSingleton<ICommandManager, CommandManager>();
            serviceCollection.AddSingleton<IHtmlDocument, HtmlDocument>();
            serviceCollection.AddSingleton<IHtmlToPdfConverter, HtmlToPdfConverter>();

            // Register all ICommand objects with exception of the base class
            serviceCollection.RegisterAllTypes<ICommand>(
                new[] { typeof(CommandManager).Assembly },
                ServiceLifetime.Transient,
                new[] { "CommandBase" }
            );

            return serviceCollection;
        }
    }
}
