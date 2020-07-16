using System;
using System.Collections.Generic;
using System.Text;

namespace PdfCreator.Library.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }
        public int IndentMultiplierForMarginSize { get; set; }
    }
}
