using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Configuration;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class IndentCommand : CommandBase, ICommand
    {
        public override string Name { get => ".indent"; }
        public override string HtmlTagToEmit { get => "p"; }
        public override string HtmlTagStyles { get => "margin-left:{{n}}em"; }

        public IndentCommand(IHtmlDocument htmlDocument, IAppConfiguration appConfiguration) : base(htmlDocument)
        {
            _appConfiguration = appConfiguration;
        }

        void ICommand.Execute(params string[] args)
        {
            if (args.Length > 0)
            {
                string marginSize = GetCalculatedMarginSizeForIndent(args[0]).ToString();

                _htmlDocument.CloseCurrentContainerNode();
                _htmlDocument.OpenNewContainerNode(HtmlTagToEmit);
                _htmlDocument.UpdateCurrentContainerNodeAttributes("style", HtmlTagStyles.Replace("{{n}}", marginSize));
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

        private readonly IAppConfiguration _appConfiguration;
    }
}
