using System;
using Microsoft.Extensions.CommandLineUtils;

namespace DotNet.Ildasm
{
    public class CommandHandler
    {
        private readonly Func<CommandArgument, int> _executor;
        private CommandLineApplication _commandLineApplication;
        private Func<int> _showHelp;

        public CommandHandler(Func<CommandArgument, int> executor, Func<int> showHelp = null)
        {
            Init();
            
            _executor = executor;
            _showHelp = showHelp;
        }

        void Init()
        {
            _commandLineApplication = new CommandLineApplication();

            _commandLineApplication.FullName = "dotnet ildasm";
            _commandLineApplication.Name = "dotnet ildasm";
            _commandLineApplication.Description = "Compare the IL difference between two .NET assemblies.";
            _commandLineApplication.HelpOption("-?|-h|--help");
            _commandLineApplication.VersionOption("-v|--version", () => 
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

            var assembly = _commandLineApplication.Argument("assembly1", "Assembly file path.", false);
            
            var output = _commandLineApplication.Option("-o|--output",
                "Save output to file.",
                CommandOptionType.SingleValue);
            
            var item = _commandLineApplication.Option("-i|--item",
                "Select a method or class to be compared.",
                CommandOptionType.SingleValue);
            
            var force = _commandLineApplication.Option("-f|--force",
                "Force output file to be overwritten.",
                CommandOptionType.NoValue);

            _commandLineApplication.OnExecute(() =>
            {
                if (!string.IsNullOrEmpty(assembly.Value))
                {
                    var arguments = new CommandArgument();
                    arguments.Assembly = assembly.Value;

                    if (item.HasValue())
                        arguments.Item = item.Value();

                    if (output.HasValue())
                        arguments.OutputFile = output.Value();

                    if (force.HasValue())
                        arguments.ForceOverwrite = true;
                    
                    _executor.Invoke(arguments);
                    return 0;
                }

                _showHelp?.Invoke();

                _commandLineApplication.ShowHelp();
                return -1;
            });
        }

        public int Handle(string[] rawArguments)
        {
            return _commandLineApplication.Execute(rawArguments);
        }
    }
}