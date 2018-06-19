using DotNet.Ildasm.Configuration;
using Xunit;

namespace DotNet.Ildasm.Tests.Configuration
{
    public class CommandOptionsShould
    {
        [Fact]
        public void Generate_OutputPath_Based_On_FileName()
        {
            var options = new CommandOptions { FilePath = "MyAssembly.dll" };

            Assert.Null(options.OutputPath);
        }

        [Fact]
        public void Not_Override_OutputPath_When_One_Is_Set()
        {
            var options = new CommandOptions { FilePath = "MyAssembly.dll", OutputPath = "OutputFile.il"};

            Assert.Equal("OutputFile.il", options.OutputPath);
        }
    }
}
