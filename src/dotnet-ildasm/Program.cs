using System.IO;
using CommandLine;
using DotNet.Ildasm.Adapters;

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
            else
            {
                if (string.IsNullOrEmpty(options.OutputPath))
                    options.OutputPath = Path.GetFileNameWithoutExtension(options.FilePath) + ".il";

                outputWriter = new FileOutputWriter(options.OutputPath);
            }

            var itemFilter = new ItemFilter(options.ItemFilter);
            new Disassembler(outputWriter, options, itemFilter, new CilHelper()).Execute();

            return 0;
        }
    }
}