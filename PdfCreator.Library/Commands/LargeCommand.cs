using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class LargeCommand : CommandBase, ICommand
    {
        public override string Name { get => ".large"; }
        public override string HtmlTagToEmit { get => "h1"; }

        public LargeCommand(IHtmlDocument htmlDocument) : base(htmlDocument) { }
    }
}
