using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace PdfCreator.Library.Commands
{
    public abstract class CommandBase : ICommand
    {
        public virtual string Name { get; }
        public virtual string HtmlTagToEmit { get; }
        public virtual string HtmlTagStyles { get; }

        public CommandBase(IHtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        /// <summary>
        /// Provide the default/base behaviour for a command which is to call the Command 'receiver' business object or htmlDocument and ask it to open a new node
        /// </summary>
        /// <param name="args"></param>
        void ICommand.Execute(params string[] args)
        {
            OpenNewContainerNode();
        }

        public void OpenNewContainerNode()
        {
            string attributeName = HtmlTagStyles != null ? "style" : null;
            _htmlDocument.OpenNewContainerNode(HtmlTagToEmit, attributeName, HtmlTagStyles);
        }

        protected IHtmlDocument _htmlDocument;
    }
}
