using PdfCreator.Library.Commands.Interfaces;
using PdfCreator.Library.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PdfCreator.Library
{
    public class CommandManager : ICommandManager
    {
        /// <summary>
        /// CommandManager Constructor
        /// </summary>
        /// <remarks>Initialises a command lookup member</remarks>
        /// <param name="commands">The DI framework provides all commands that implement ICommand</param>
        public CommandManager(IHtmlDocument htmlDocument, IEnumerable<ICommand> commands)
        {
            _htmlDocument = htmlDocument;

            foreach (ICommand command in commands)
            {
                _commandLookup[command.Name] = command;
            }
        }

        /// <summary>
        /// Iterates over the input data and invokes the required command on each line
        /// </summary>
        /// <param name="inputData">collection of lines of input data passed to the PdfCreator program</param>
        public string Execute(IEnumerable<string> inputDataLines)
        {
            _htmlDocument.Initialise();

            foreach (string line in inputDataLines)
            {
                string commandName = this.GetCommandName(line);
                ICommand command = IsCommandRecognised(commandName) ? _commandLookup[commandName] : _commandLookup["{Content}"];
                command.Execute(line);
            }
            return _htmlDocument.OuterHtml;
        }

        #region Private methods / members

        private bool IsCommandRecognised(string commandName)
        {
            return _commandLookup.ContainsKey(commandName);
        }

        private string GetCommandName(string line)
        {
            string commandName = line?.Trim()?.Split(' ').FirstOrDefault();
            return commandName.ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }

        private readonly Dictionary<string, ICommand> _commandLookup = new Dictionary<string, ICommand>();
        private readonly IHtmlDocument _htmlDocument;

        #endregion
    }
}
