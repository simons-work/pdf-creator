using PdfCreator.Library.Configuration;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var pdfCreator = AppInitialisation.GetRequiredService<IPdfCreator>(args);
            pdfCreator.CreatePdfOutputFromCommandInput();
        }
    }
}
