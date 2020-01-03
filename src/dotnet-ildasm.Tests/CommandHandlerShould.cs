using System;
using DotNet.Ildasm;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests
{
    public class CommandHandlerShould
    {
        private Func<CommandArgument, int> _executor;
        private Func<int> _showHelp;

        public CommandHandlerShould()
        {
            _executor = Substitute.For<Func<CommandArgument, int>>();
            _showHelp = Substitute.For<Func<int>>();
        }

        [Fact]
        public void Execute_Command_With_Assembly()
        {
            var arguments = new string[] { "assembly1.dll" };
            var handler = new CommandHandler(_executor, _showHelp);
            var expected = new CommandArgument
            {
                Assembly = "assembly1.dll"
            };

            handler.Handle(arguments);

            _executor.Received(1).Invoke(Arg.Is<CommandArgument>(x =>
                x.Assembly == expected.Assembly));
        }

        [Fact]
        public void Execute_Command_With_Assembly_With_Output_File()
        {
            var arguments = new string[] { "assembly1.dll", "-o", "output.il" };
            var handler = new CommandHandler(_executor, _showHelp);
            var expected = new CommandArgument
            {
                Assembly = "assembly1.dll",
                OutputFile = "output.il"
            };

            handler.Handle(arguments);

            _executor.Received(1).Invoke(Arg.Is<CommandArgument>(x =>
                x.Assembly == expected.Assembly &&
                x.OutputFile == expected.OutputFile));
        }

        [Fact]
        public void Execute_Command_With_Assembly_And_Item_With_Output_File()
        {
            var arguments = new string[] { "assembly1.dll", "-o", "output.il", "-i", "::Method" };
            var handler = new CommandHandler(_executor, _showHelp);
            var expected = new CommandArgument
            {
                Assembly = "assembly1.dll",
                OutputFile = "output.il",
                Item = "::Method"
            };

            handler.Handle(arguments);

            _executor.Received(1).Invoke(Arg.Is<CommandArgument>(x =>
                x.Assembly == expected.Assembly &&
                x.OutputFile == expected.OutputFile &&
                x.Item == expected.Item));
        }

        [Fact]
        public void Execute_Command_With_Assembly_And_Item()
        {
            var arguments = new string[] { "assembly1.dll", "-i", "::Method" };
            var handler = new CommandHandler(_executor, _showHelp);
            var expected = new CommandArgument
            {
                Assembly = "assembly1.dll",
                Item = "::Method"
            };

            handler.Handle(arguments);

            _executor.Received(1).Invoke(Arg.Is<CommandArgument>(x =>
                x.Assembly == expected.Assembly &&
                x.Item == expected.Item));
        }

        [Fact]
        public void Print_Help_If_No_Arguments()
        {
            var arguments = new string[] { };
            var handler = new CommandHandler(_executor, _showHelp);

            handler.Handle(arguments);

            _showHelp.Received(1).Invoke();
        }
    }
}