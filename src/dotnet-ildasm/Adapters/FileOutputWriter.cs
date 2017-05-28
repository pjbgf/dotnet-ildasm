using System;
using System.IO;

namespace DotNet.Ildasm.Adapters
{
    public sealed class FileOutputWriter : IOutputWriter
    {
        private readonly IndentationProvider _indentationProvider;
        private readonly string _filePath;

        internal FileOutputWriter(IndentationProvider indentationProvider, string filePath)
        {
            _indentationProvider = indentationProvider;
            this._filePath = filePath;
        }

        public void Write(string value)
        {
            File.AppendAllText(_filePath, value);
        }

        public void WriteLine(string value = "")
        {
            File.AppendAllText(_filePath, $"{_indentationProvider.Apply(value)}{Environment.NewLine}");
        }
    }
}