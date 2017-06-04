using System;
using System.IO;
using System.Text;

namespace DotNet.Ildasm.Adapters
{
    public sealed class FileOutputWriter : IOutputWriter
    {
        private readonly StringBuilder _stringBuilder;
        private readonly IndentationProvider _indentationProvider;
        private readonly string _filePath;

        public FileOutputWriter(IndentationProvider indentationProvider, string filePath)
        {
            _stringBuilder = new StringBuilder();
            _indentationProvider = indentationProvider;
            this._filePath = filePath;
        }

        public void Write(string value, bool indentCode = false)
        {
            _stringBuilder.Append(indentCode ? $"{_indentationProvider.Apply(value)}" : value);
        }

        public void WriteLine(string value = "")
        {
            _stringBuilder.AppendLine($"{_indentationProvider.Apply(value)}{Environment.NewLine}");
        }

        public void Dispose()
        {
            File.WriteAllText(_filePath, _stringBuilder.ToString());
            _stringBuilder.Clear();
        }
    }
}