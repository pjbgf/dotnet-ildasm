using System.Linq;
using DotNet.Ildasm.Infrastructure;
using DotNet.Ildasm.Tests.Internal;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class AssemblyNameReferenceExtensionsShould
    {
#if NETFRAMEWORK

        [Fact]
        public void Write_AssemblyExtern_For_NetFramework45()
        {
            var outputWriterMock = Substitute.For<IOutputWriter>();
            var reference = DataHelper.SampleAssembly.Value.MainModule
                .AssemblyReferences.First(x => x.Name == "System.Console");

            reference.WriteIL(outputWriterMock);

            Received.InOrder(() =>
            {
                outputWriterMock.WriteLine(".assembly extern System.Console");
                outputWriterMock.WriteLine("{");
                outputWriterMock.WriteLine(".publickeytoken = ( B0 3F 5F 7F 11 D5 0A 3A ) // .......Q");
                outputWriterMock.WriteLine(".ver 4:0:0:0");
                outputWriterMock.WriteLine("}");
            });
        }

#endif

        [Fact]
        public void Write_AssemblyExtern_For_NetLibrary20()
        {
            var outputWriterMock = Substitute.For<IOutputWriter>();
            var reference = DataHelper.SampleAssembly.Value.MainModule
                .AssemblyReferences.First(x => x.Name == "netstandard");

            reference.WriteIL(outputWriterMock);

            Received.InOrder(() =>
            {
                outputWriterMock.WriteLine(".assembly extern netstandard");
                outputWriterMock.WriteLine("{");
                outputWriterMock.WriteLine(".publickeytoken = ( CC 7B 13 FF CD 2D DD 51 ) // .......Q");
                outputWriterMock.WriteLine("}");
            });
        }
    }
}