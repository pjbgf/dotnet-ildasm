using System;
using System.IO;

namespace DotNet.Ildasm
{
    internal sealed class FileOutputWriter : IOutputWriter
    {
        private readonly string _filePath;

        internal FileOutputWriter(string filePath)
        {
            this._filePath = filePath;
        }

        public void Write(string value)
        {
            File.AppendAllText(_filePath, value);
        }

        public void WriteLine(string value = "")
        {
            File.AppendAllText(_filePath, $"{value}{Environment.NewLine}");
        }
    }
}