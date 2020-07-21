using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System.Xml.Linq;

namespace PdfCreator.Library.Commands
{
    public class LargeCommand : ICommand
    {
        public string Name { get => ".large"; }
        public string HtmlTagToEmit { get => "h1"; }

        public LargeCommand(IHtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        void ICommand.Execute(params string[] args)
        {
            XElement element = _htmlDocument.CreateDocumentNode(HtmlTagToEmit);
            _htmlDocument.AddDocumentChildNode(element, true);
        }

        private IHtmlDocument _htmlDocument;
    }
}
