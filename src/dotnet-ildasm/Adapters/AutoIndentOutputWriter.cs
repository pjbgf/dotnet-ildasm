using System;
using System.Text.RegularExpressions;

namespace DotNet.Ildasm
{
    public class AutoIndentOutputWriter : IOutputWriter
    {
        private readonly IOutputWriter _writer;
        private int _numSpaces = 2;
        private int _currentLevel = 0;

        public AutoIndentOutputWriter(IOutputWriter writer)
        {
            _writer = writer;
        }

        public void Dispose()
        {
        }

        public void Write(string value)
        {
            Apply(value);
        }

        public void WriteLine(string value)
        {
            Apply(value);
            _writer.WriteLine(string.Empty);
        }

        public void Apply(string code)
        {
            //TODO: Refactor / Optimise
            var alreadyUpdatedIndentation = false;
            if (code.StartsWith(".method") || code.StartsWith(".field") || code.StartsWith(".class") || code.StartsWith(".assembly") || code.StartsWith(".module"))
            {
                _writer.WriteLine(string.Empty);
            }

            if (code.StartsWith("}"))
            {
                alreadyUpdatedIndentation = true;
                UpdateIndentationLevel(code);
            }
            code = code.Replace("{", Environment.NewLine + "{");

            var totalIndentation = _currentLevel * _numSpaces;
            _writer.Write(code.PadLeft(code.Length + totalIndentation));
            
            if (!alreadyUpdatedIndentation)
                UpdateIndentationLevel(code);
        }

        private void UpdateIndentationLevel(string code)
        {
            var openBracketMatches = Regex.Matches(code, "{");
            var closeBracketMatches = Regex.Matches(code, "}");

            var delta = openBracketMatches.Count - closeBracketMatches.Count;
            _currentLevel += delta;

            if (_currentLevel < 0)
                _currentLevel = 0;
        }
    }
}