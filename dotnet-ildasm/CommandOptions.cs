using CommandLine;
using System.Collections.Generic;

namespace DotNet.Ildasm
{
    internal class CommandOptions
    {
        [Option('t', "text", Required = false, HelpText = "Output results to the console.")]
        public bool IsTextOutput { get; set; }

        [Value(index: 0, Required = true, MetaName = "filepath", HelpText = "Path to the Portable Executable to be disassembled.")]
        public string FilePath { get; set; }

    }
}
