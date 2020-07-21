using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System.Xml.Linq;

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
            XElement element = _htmlDocument.CreateDocumentNode(HtmlTagToEmit);
            _htmlDocument.AddDocumentChildNode(element, true);
            // not really necessary for the output as the pdf creator copes with funny xhtml such as <p/> but this forces it to be <p></p> and only needed if 
            // someone did a .nofill after an empty paragraph which is the default for a new empty para anyway so a bit pointless!
            element.Add(new XText(""));
        }

        private IHtmlDocument _htmlDocument;
    }
}
