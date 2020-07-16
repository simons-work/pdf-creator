using System.Collections.Generic;

namespace PdfCreator.Library.Interfaces
{
    public interface ICommandManager
    {
        string Execute(IEnumerable<string> inputData);
    }
}