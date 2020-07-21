using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Configuration;
using PdfCreator.Library.Interfaces;
using System.Xml.Linq;

namespace PdfCreator.Library.Commands
{
    public class IndentCommand : ICommand
    {
        public string Name { get => ".indent"; }
        public string HtmlTagToEmit { get => "p"; }
        public string HtmlTagStyles { get => "margin-left:{{n}}em"; }

        public IndentCommand(IHtmlDocument htmlDocument, IAppConfiguration appConfiguration)
        {
            _htmlDocument = htmlDocument;
            _appConfiguration = appConfiguration;
        }

        void ICommand.Execute(params string[] args)
        {
            if (args.Length > 0)
            {
                _htmlDocument.CloseCurrentContainerNode();
                XElement element = _htmlDocument.CreateDocumentNode(HtmlTagToEmit);

                string existingAttribute = element.Attribute("style")?.Value;
                string marginSize = GetCalculatedMarginSizeForIndent(args[0]).ToString();
                string htmlTagStyles = HtmlTagStyles.Replace("{{n}}", marginSize);
                element.SetAttributeValue("style", string.Join(";", htmlTagStyles, existingAttribute));

                _htmlDocument.AddDocumentChildNode(element, true);
            }
        }

        private int GetCalculatedMarginSizeForIndent(string inputLine)
        {
            var indentArugments = inputLine?.Trim()?.Split(' ');
            if (indentArugments.Length > 1)
            {
                int requestedIndent;
                int.TryParse(indentArugments[1], out requestedIndent);
                _htmlDocument.CurrentIndentation += requestedIndent;
                _htmlDocument.CurrentIndentation = _htmlDocument.CurrentIndentation >= 0 ? _htmlDocument.CurrentIndentation : 0;
            }
            return _htmlDocument.CurrentIndentation * _appConfiguration.IndentMultiplierForMarginSize;
        }

        private IHtmlDocument _htmlDocument;
        private readonly IAppConfiguration _appConfiguration;
    }
}
