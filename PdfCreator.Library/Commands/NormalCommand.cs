using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System;
using System.Xml;

namespace PdfCreator.Library.Commands
{
    public class NormalCommand : ICommand
    {
        public string Name { get => ".normal"; }

        public NormalCommand(IHtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        void ICommand.Execute(params string[] args)
        {
            _htmlDocument.CloseCurrentContainerNode();
        }

        private Guid _id = Guid.NewGuid();
        private IHtmlDocument _htmlDocument;
    }
}
