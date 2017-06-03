using System.Text;

namespace DotNet.Ildasm.Tests.Internal
{
    internal class OutputWriterDouble : IOutputWriter
    {
        private StringBuilder _stringBuilder;

        public OutputWriterDouble()
        {
            _stringBuilder = new StringBuilder();
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        public void Write(string value)
        {
            _stringBuilder.Append(value);
        }

        public void WriteLine(string value = "")
        {
            _stringBuilder.AppendLine(value);
        }
    }
}