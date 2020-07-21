using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class ItalicCommand : CommandBase, ICommand
    {
        public override string Name { get => ".italics"; }
        public override string HtmlTagToEmit { get => "span"; }
        public override string HtmlTagStyles { get => "font-style:italic"; }

        public ItalicCommand(IHtmlDocument htmlDocument) : base(htmlDocument) { }
    }
}
