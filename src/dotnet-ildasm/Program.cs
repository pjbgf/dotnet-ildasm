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
            var indentationProvider = new IndentationProvider();
            IOutputWriter outputWriter = null;

            if (options.IsTextOutput)
            {
                outputWriter = new ConsoleOutputWriter(indentationProvider);
            }
            else
            {
                if (string.IsNullOrEmpty(options.OutputPath))
                    options.OutputPath = Path.GetFileNameWithoutExtension(options.FilePath) + ".il";

                outputWriter = new FileOutputWriter(indentationProvider, options.OutputPath);
            }

            var itemFilter = new ItemFilter(options.ItemFilter);
            var assemblyDataProcessor = new AssemblyDataProcessor(options.FilePath, outputWriter);
            var assemblyDefinitionResolver = new AssemblyDefinitionResolver();
            
            new Disassembler(assemblyDataProcessor, assemblyDefinitionResolver).Execute(options, itemFilter);

            return 0;
        }
    }
}