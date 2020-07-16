using System;
using System.Collections.Generic;
using System.Text;

namespace PdfCreator.Library.Commands.Interfaces
{
    public interface ICommand
    {
        public string Name { get; }
        public void Execute(params string[] args);
    }
}
