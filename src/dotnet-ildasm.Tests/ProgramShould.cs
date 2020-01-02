using System;
using DotNet.Ildasm.Configuration;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests
{
    public class ProgramShould
    {
        [Fact]
        public void Abort_If_No_Parameters_Are_Sent()
        {
            var mock = Substitute.For<IOutputWriter>();
            var program = new Program(mock);
            var returnCode = program.Execute(new string[0]);

            Assert.Equal(-1, returnCode);
        }
    }
}