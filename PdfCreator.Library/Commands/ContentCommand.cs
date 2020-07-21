using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System.Xml.Linq;

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
                XNode element = _htmlDocument.CreateContentNode($"{args[0]} ");
                _htmlDocument.AddDocumentChildNode(element, false);
            }
        }

        private IHtmlDocument _htmlDocument;
    }
}
