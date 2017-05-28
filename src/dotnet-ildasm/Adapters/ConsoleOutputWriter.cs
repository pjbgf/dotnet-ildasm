using System;

namespace DotNet.Ildasm.Adapters
{
    public sealed class ConsoleOutputWriter : IOutputWriter
    {
        private readonly IndentationProvider _indentationProvider;

        public ConsoleOutputWriter(IndentationProvider indentationProvider)
        {
            _indentationProvider = indentationProvider;
        }
        
        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(_indentationProvider.Apply(value));
        }
    }
}