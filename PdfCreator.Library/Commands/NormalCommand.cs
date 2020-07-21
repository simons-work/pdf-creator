using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class NormalCommand : CommandBase, ICommand
    {
        public override string Name { get => ".normal"; }

        public NormalCommand(IHtmlDocument htmlDocument) : base(htmlDocument) { }

        void ICommand.Execute(params string[] args)
        {
            _htmlDocument.CloseCurrentContainerNode();
        }
    }
}
