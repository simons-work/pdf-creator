using PdfCreator.Library.Configuration;
using PdfCreator.Library.Interfaces;
using System.IO;

namespace PdfCreator.Library
{
    public class PdfCreator : IPdfCreator
    {
        public PdfCreator(IAppConfiguration appConfiguration, ICommandManager commandManager, IHtmlToPdfConverter htmlToPdfConverter, IHtmlDocument htmlDocument)
        {
            _appConfiguration = appConfiguration;
            _commandManager = commandManager;
            _htmlToPdfConverter = htmlToPdfConverter;
            _htmlDocument = htmlDocument;
        }

        public void CreatePdfOutputFromCommandInput()
        {
            var inputData = File.ReadAllLines(_appConfiguration.InputFilename);

            var htmlFragment = _commandManager.Execute(inputData);
            var htmlDocument = _htmlDocument.PopulateHtmlTemplate(htmlFragment);
            var pdfByteArray = _htmlToPdfConverter.Convert(htmlDocument);

            File.WriteAllBytes(_appConfiguration.OutputFilename, pdfByteArray);
        }

        private readonly IAppConfiguration _appConfiguration;
        private readonly ICommandManager _commandManager;
        private readonly IHtmlToPdfConverter _htmlToPdfConverter;
        private readonly IHtmlDocument _htmlDocument;
    }
}
