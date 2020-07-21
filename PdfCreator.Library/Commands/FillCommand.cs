using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System.Xml.Linq;

namespace PdfCreator.Library.Commands
{
    public class FillCommand : ICommand
    {
        public string Name { get => ".fill"; }
        public string HtmlTagToEmit { get => "p"; }

        public string HtmlTagStyles { get => "text-align: justify"; }

        public FillCommand(IHtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        void ICommand.Execute(params string[] args)
        {
            XElement currentContainer = _htmlDocument.CurrentContainer;
            
            if (!string.IsNullOrEmpty(currentContainer.Value)) 
            {
                _htmlDocument.CloseCurrentContainerNode();
                XElement element = _htmlDocument.CreateDocumentNode(HtmlTagToEmit);
                _htmlDocument.AddDocumentChildNode(element, true);
            }

            // Grab the container again as will have changed if the AddDocumentChildNode was invoked above
            currentContainer = _htmlDocument.CurrentContainer;
            string existingAttribute = currentContainer.Attribute("style")?.Value;
            currentContainer.SetAttributeValue("style", string.Join(";", HtmlTagStyles, existingAttribute));
        }

        private IHtmlDocument _htmlDocument;
    }
}
