using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System;
using System.Xml;

namespace PdfCreator.Library.Commands
{
    public class BoldCommand : ICommand
    {
        public string Name { get => ".bold"; }
        public string HtmlTagToEmit { get => "span"; }
        public string HtmlTagStyles { get => "font-weight:bold"; }

        public BoldCommand(IHtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        void ICommand.Execute(params string[] args)
        {
            XmlElement element =_htmlDocument.CreateDocumentNode(HtmlTagToEmit);
            element.SetAttribute("style", HtmlTagStyles);
            _htmlDocument.AddDocumentChildNode(element, true);
        }

        private Guid _id = Guid.NewGuid();
        private IHtmlDocument _htmlDocument;
    }
}
