using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PdfCreator.Library.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }
        public int IndentMultiplierForMarginSize { get; set; }

        public void Load(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            configuration.Bind("config", this);
        }
    }
}
