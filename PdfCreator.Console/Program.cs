using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PdfCreator.Library;
using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Configuration;
using PdfCreator.Library.Extensions;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();            
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var appConfiguration = serviceProvider.GetRequiredService<IAppConfiguration>();
            var pdfCreator = serviceProvider.GetRequiredService<IPdfCreator>();

            LoadConfiguration(args, appConfiguration);
            pdfCreator.CreatePdfOutputFromCommandInput();
        }

        static private void LoadConfiguration(string[] args, IAppConfiguration appConfiguration)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            configuration.Bind("config", appConfiguration);
        }

        static private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAppConfiguration, AppConfiguration>();
            serviceCollection.AddSingleton<IPdfCreator, PdfCreator.Library.PdfCreator>();
            serviceCollection.AddSingleton<ICommandManager, CommandManager>();
            serviceCollection.AddSingleton<IHtmlDocument, HtmlDocument>();
            serviceCollection.AddSingleton<IHtmlToPdfConverter, HtmlToPdfConverter>();

            // Register all ICommand objects with exception of the base class
            serviceCollection.RegisterAllTypes<ICommand>(
                new[] { typeof(CommandManager).Assembly },
                ServiceLifetime.Transient,
                new[] { "CommandBase" }
            );
        }
    }
}
