using System;
using System.IO;
using System.Text;

namespace DotNet.Ildasm.Adapters
{
    internal sealed class FileStreamOutputWriter : IOutputWriter, IDisposable
    {
        private readonly FileStream _stream;

        public FileStreamOutputWriter(string filePath)
        {
            _stream = File.Open(filePath, FileMode.OpenOrCreate);
        }

        public void Dispose()
        {
            _stream?.Flush();
            _stream?.Dispose();
        }

        public void Write(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            _stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteLine(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            _stream.Write(bytes, 0, bytes.Length);
        }
    }
}