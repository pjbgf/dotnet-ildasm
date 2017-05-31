using System;
using System.IO;
using System.Text;

namespace DotNet.Ildasm.Adapters
{
    public sealed class FileStreamOutputWriter : IOutputWriter, IDisposable
    {
        FileStream stream;
        private readonly StringBuilder _stringBuilder;
        private readonly IndentationProvider _indentationProvider;
        private readonly string _filePath;

        public FileStreamOutputWriter(IndentationProvider indentationProvider, string filePath)
        {
            stream = File.Open(filePath, FileMode.OpenOrCreate);
            var bufferedStream = new BufferedStream(stream);
            _stringBuilder = new StringBuilder();

            var memoryStream = new MemoryStream();
         
            _indentationProvider = indentationProvider;
            this._filePath = filePath;
        }

        public void Dispose()
        {
            if (stream != null)
                stream.Dispose();
        }

        public void Write(string value)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteLine(string value = "")
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes($"{_indentationProvider.Apply(value)}{Environment.NewLine}");
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}