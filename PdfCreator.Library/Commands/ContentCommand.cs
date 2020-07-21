using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class ContentCommand : CommandBase, ICommand
    {
        // This command name will never exist in the input file so it's just a made up command
        // so we can use the same command lookup dictionary for all commands
        public override string Name { get => "{Content}"; }

        public ContentCommand(IHtmlDocument htmlDocument) : base(htmlDocument) { }

        void ICommand.Execute(params string[] args)
        {
            if (args.Length > 0)
            {
                _htmlDocument.AddNewContentNode($"{args[0]} ");
            }
        }
    }
}
