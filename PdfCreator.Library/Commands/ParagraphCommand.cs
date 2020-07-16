using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PdfCreator.Library.Commands
{
    public class ParagraphCommand : ICommand
    {
        public string Name { get => ".paragraph"; }
        public string HtmlTagToEmit { get => "p"; }

        public ParagraphCommand(IHtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        void ICommand.Execute(params string[] args)
        {
            XmlElement element = _htmlDocument.CreateDocumentNode(HtmlTagToEmit);
            _htmlDocument.AddDocumentChildNode(element, true);
        }

        private Guid _id = Guid.NewGuid();
        private IHtmlDocument _htmlDocument;
    }
}
