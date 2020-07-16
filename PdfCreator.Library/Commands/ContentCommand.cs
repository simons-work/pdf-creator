using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System;
using System.Xml;

namespace PdfCreator.Library.Commands
{
    public class ContentCommand : ICommand
    {
        public string Name { get => "{Content}"; }

        public ContentCommand(IHtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        void ICommand.Execute(params string[] args)
        {
            if (args.Length > 0)
            {
                XmlText element = _htmlDocument.CreateContentNode($"{args[0]} ");
                _htmlDocument.AddDocumentChildNode(element, false);
            }
        }

        private Guid _id = Guid.NewGuid();
        private IHtmlDocument _htmlDocument;
    }
}
