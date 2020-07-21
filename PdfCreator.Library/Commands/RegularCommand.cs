using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class RegularCommand : CommandBase, ICommand
    {
        public override string Name { get => ".regular"; }

        public RegularCommand(IHtmlDocument htmlDocument) : base(htmlDocument) { }

        void ICommand.Execute(params string[] args)
        {
            _htmlDocument.CloseCurrentContainerNode();
        }
    }
}
