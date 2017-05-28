using System.Text.RegularExpressions;

namespace DotNet.Ildasm
{
    public class IndentationProvider
    {
        private int _numSpaces = 2;
        private int _currentLevel = 0;
        
        public string Apply(string code)
        {
            var alreadyUpdatedIndentation = false;
            if (code.StartsWith("}"))
            {
                alreadyUpdatedIndentation = true;
                UpdateIndentationLevel(code);   
            }
                
            var totalIndentation = _currentLevel * _numSpaces;
            var indentedCode = code.PadLeft(code.Length + totalIndentation);
            
            if (!alreadyUpdatedIndentation)
                UpdateIndentationLevel(code);
            
            return indentedCode;
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