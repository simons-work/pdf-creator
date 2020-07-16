using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System;
using System.Xml;

namespace PdfCreator.Library.Commands
{
    public class FillCommand : ICommand
    {
        public string Name { get => ".fill"; }
        public string HtmlTagStyles { get => "text-align: justify"; }

        public FillCommand(IHtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        void ICommand.Execute(params string[] args)
        {
            XmlElement currentContainer = _htmlDocument.CurrentContainer;
            string existingAttribute = currentContainer.GetAttribute("style");
            currentContainer.SetAttribute("style", string.Join(";", HtmlTagStyles, existingAttribute));
        }

        private Guid _id = Guid.NewGuid();
        private IHtmlDocument _htmlDocument;
    }
}
