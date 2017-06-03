using System.Linq;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class AssemblyNameReferenceExtensionsShould
    {
        [Fact]
        public void Write_IL()
        {
            var outputWriterMock = Substitute.For<IOutputWriter>();
            var reference = DataHelper.SampleAssembly.Value.MainModule
                .AssemblyReferences.First(x => x.Name == "System.Console");

            reference.WriteIL(outputWriterMock);

            Received.InOrder(() =>
            {
                outputWriterMock.WriteLine(".assembly extern System.Console");
                outputWriterMock.WriteLine("{");
                outputWriterMock.WriteLine(".publickeytoken = ( B0 3F 5F 7F 11 D5 0A 3A )");
                outputWriterMock.WriteLine(".ver 4:0:0:0");
                outputWriterMock.WriteLine("}");
            });
        }
    }
}