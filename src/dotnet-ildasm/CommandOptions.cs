using System.IO;
using CommandLine;

namespace DotNet.Ildasm
{
    public class CommandOptions
    {
        private string _outputPath;

        [Option("force", Required = false, HelpText = "Force an existing output file to be overwritten.")]
        public bool ForceOutputOverwrite { get; set; }

        [Option('t', "text", Required = false, HelpText = "Output results to the console.")]
        public bool IsTextOutput { get; set; }

        [Option('o', "output", Required = false, HelpText = "File path to be used as output.")]
        public string OutputPath {
            get
            {
                if (!this.IsTextOutput && string.IsNullOrEmpty(_outputPath))
                    _outputPath = Path.GetFileNameWithoutExtension(this.FilePath) + ".il";

                return _outputPath;
            }
            set
            {
                _outputPath = value;
            }
        }

        [Option('i', "item", Required = false, HelpText = "Filter to define which item(s) will be processed.")]
        public string ItemFilter { get; set; }

        [Value(index: 0, Required = true, MetaName = "filepath", HelpText = "Path to the Portable Executable to be disassembled.")]
        public string FilePath { get; set; }
    }
}