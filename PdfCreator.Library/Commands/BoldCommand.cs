using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class BoldCommand : CommandBase, ICommand
    {
        public override string Name { get => ".bold"; }
        public override string HtmlTagToEmit { get => "span"; }
        public override string HtmlTagStyles { get => "font-weight:bold"; }

        public BoldCommand(IHtmlDocument htmlDocument) : base(htmlDocument) { }
    }
}
