using System;

namespace DotNet.Ildasm
{
    internal class ConsoleOutputWriter : IOutputWriter
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