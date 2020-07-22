using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;

namespace PdfCreator.Library.Commands
{
    public class FillCommand : CommandBase, ICommand
    {
        public override string Name { get => ".fill"; }
        public override string HtmlTagToEmit { get => "p"; }

        public override string HtmlTagStyles { get => "text-align: justify"; }

        public FillCommand(IHtmlDocument htmlDocument) : base(htmlDocument) { }

        /// <summary>
        /// Fill command Execute behaviour will just udpate the existing container style with justification style if it's empty, otherwise it will start a new container so it can have it's style set independently
        /// </summary>
        /// <param name="args"></param>
        void ICommand.Execute(params string[] args)
        {
            if (!_htmlDocument.IsCurrentContainerNodeEmpty) 
            {
                _htmlDocument.CloseCurrentContainerNode();
                _htmlDocument.OpenNewContainerNode(HtmlTagToEmit);
            }

            _htmlDocument.UpdateCurrentContainerNodeAttributes("style", HtmlTagStyles);
        }
    }
}
