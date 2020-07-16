using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System;
using System.Xml;

namespace PdfCreator.Library.Commands
{
    public class ItalicCommand : ICommand
    {
        public string Name { get => ".italics"; }
        public string HtmlTagToEmit { get => "span"; }
        public string HtmlTagStyles { get => "font-style:italic"; }

        public ItalicCommand(IHtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        void ICommand.Execute(params string[] args)
        {
            XmlElement element =_htmlDocument.CreateDocumentNode(HtmlTagToEmit);
            element.SetAttribute("style", HtmlTagStyles);
            _htmlDocument.AddDocumentChildNode(element, true);
        }

        private IHtmlDocument _htmlDocument;
    }
}
