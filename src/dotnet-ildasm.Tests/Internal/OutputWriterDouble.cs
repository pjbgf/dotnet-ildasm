﻿using System;
using System.Text;

namespace DotNet.Ildasm.Tests.Internal
{
    internal class OutputWriterDouble : IOutputWriter
    {
        private readonly StringBuilder _stringBuilder;

        public OutputWriterDouble()
        {
            _stringBuilder = new StringBuilder();
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            var content = _stringBuilder.ToString();
            if (content.EndsWith(Environment.NewLine))
                return content.Substring(0, content.Length - Environment.NewLine.Length);

            return content;
        }

        public void Write(string value)
        {
            _stringBuilder.Append(value);
        }

        public void WriteLine(string value)
        {
            _stringBuilder.AppendLine(value);
        }
    }
}