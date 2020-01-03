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
            _writer?.Dispose();
        }

        public void Write(string value)
        {
            Apply(value);
        }

        public void WriteLine(string value)
        {
            Apply(value);
            _writer.Write(Environment.NewLine);
        }

        public void Apply(string code)
        {
            var alreadyUpdatedIndentation = false;

            if (IsSingleLineBreakRequired(code))
                _writer.WriteLine(string.Empty);
            
            if (IsDoubleLineBreakRequired(code))
            {
                _writer.WriteLine(string.Empty);
                _writer.Write(Environment.NewLine);
            }

            if (code.StartsWith("}"))
            {
                alreadyUpdatedIndentation = true;
                UpdateIndentationLevel(code);
            }

            var totalIndentation = _currentLevel * _numSpaces;
            if (IsIndentationRequired(code))
                _writer.Write(code.PadLeft(code.Length + totalIndentation));
            else
                _writer.Write(code);

            if (!alreadyUpdatedIndentation)
                UpdateIndentationLevel(code);
        }

        private static bool IsSingleLineBreakRequired(string code)
        {
            return Regex.IsMatch(code, "^(.field|.method|.property|.event){1}",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);
        }

        private static bool IsDoubleLineBreakRequired(string code)
        {
            return Regex.IsMatch(code, "^(.class|.assembly|.module){1}",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);
        }

        private static bool IsIndentationRequired(string code)
        {
            return Regex.IsMatch(code, "^(catch|IL|\\.|//|{|}){1}",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);
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