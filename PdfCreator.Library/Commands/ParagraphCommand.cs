using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class ParagraphCommand : CommandBase, ICommand
    {
        public override string Name { get => ".paragraph"; }
        public override string HtmlTagToEmit { get => "p"; }

        public ParagraphCommand(IHtmlDocument htmlDocument) : base(htmlDocument) { }
    }
}
