using System;

namespace DotNet.Ildasm.Adapters
{
    internal sealed class ConsoleOutputWriter : IOutputWriter
    {
        private readonly IndentationProvider _indentationProvider;

        public ConsoleOutputWriter(IndentationProvider indentationProvider)
        {
            _indentationProvider = indentationProvider;
        }
        
        public void Write(string value, bool indentCode = false)
        {
            Console.Write(indentCode ? $"{_indentationProvider.Apply(value)}" : value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(_indentationProvider.Apply(value));
        }

        public void Dispose()
        {
        }
    }
}