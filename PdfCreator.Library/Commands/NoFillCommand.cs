using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class NoFillCommand : CommandBase, ICommand
    {
        public override string Name { get => ".nofill"; }
        public override string HtmlTagToEmit { get => "p"; }

        public NoFillCommand(IHtmlDocument htmlDocument) : base(htmlDocument) { }

        void ICommand.Execute(params string[] args)
        {
            _htmlDocument.CloseCurrentContainerNode();
            OpenNewContainerNode();
        }
    }
}
