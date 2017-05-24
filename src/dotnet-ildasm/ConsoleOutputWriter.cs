using System;

namespace DotNet.Ildasm
{
    internal sealed class ConsoleOutputWriter : IOutputWriter
    {
        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}