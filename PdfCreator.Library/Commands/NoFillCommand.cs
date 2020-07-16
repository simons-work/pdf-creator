using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PdfCreator.Library.Commands
{
    public class NoFillCommand : ICommand
    {
        public string Name { get => ".nofill"; }
        public string HtmlTagToEmit { get => "p"; }

        public NoFillCommand(IHtmlDocument htmlDocument)
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
