using System;
using System.IO;
using System.Text;

namespace DotNet.Ildasm.Adapters
{
    public sealed class FileStreamOutputWriter : IOutputWriter, IDisposable
    {
        FileStream stream;
        private readonly IndentationProvider _indentationProvider;

        public FileStreamOutputWriter(IndentationProvider indentationProvider, string filePath)
        {
            stream = File.Open(filePath, FileMode.OpenOrCreate);
            _indentationProvider = indentationProvider;
        }

        public void Dispose()
        {
            stream?.Dispose();
        }

        public void Write(string value, bool indentCode = false)
        {
            var valueWithIndentation = indentCode ? $"{_indentationProvider.Apply(value)}" : value;
            byte[] bytes = Encoding.ASCII.GetBytes(valueWithIndentation);
            stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteLine(string value = "")
        {
            byte[] bytes = Encoding.ASCII.GetBytes($"{_indentationProvider.Apply(value)}{Environment.NewLine}");
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}