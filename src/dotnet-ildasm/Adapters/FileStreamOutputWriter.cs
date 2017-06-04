using System;
using System.IO;
using System.Text;

namespace DotNet.Ildasm.Adapters
{
    internal sealed class FileStreamOutputWriter : IOutputWriter, IDisposable
    {
        FileStream stream;

        public FileStreamOutputWriter(string filePath)
        {
            stream = File.Open(filePath, FileMode.OpenOrCreate);
        }

        public void Dispose()
        {
            stream?.Dispose();
        }

        public void Write(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteLine(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}