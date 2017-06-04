using System;

namespace DotNet.Ildasm
{
    public interface IOutputWriter : IDisposable
    {
        void Write(string value);
        void WriteLine(string value);
    }
}