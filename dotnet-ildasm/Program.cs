using System;
using System.Linq;
using CommandLine;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<CommandOptions>(args);
            result.MapResult(
                RunIldasm,
                _ => ShowInfo());
        }

        private static int ShowInfo()
        {
            return -1;
        }

        private static int RunIldasm(CommandOptions options)
        {
            IOutputWriter outputWriter = null;

            if (options.IsTextOutput)
                outputWriter = new ConsoleOutputWriter();

            new Disassembler(outputWriter).Execute(options);

            return 0;
        }
    }
}