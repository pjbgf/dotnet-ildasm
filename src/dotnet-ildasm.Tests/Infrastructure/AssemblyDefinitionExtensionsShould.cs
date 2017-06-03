using DotNet.Ildasm.Infrastructure;
using DotNet.Ildasm.Tests.Internal;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class AssemblyDefinitionExtensionsShould
    {
        [Fact]
        public void Write_IL()
        {
            var outputWriter = Substitute.For<IOutputWriter>();
            var assemblyDefinition = DataHelper.SampleAssembly.Value;

            assemblyDefinition.WriteIL(outputWriter);

            outputWriter.Received(1).WriteLine(".assembly 'dotnet-ildasm.Sample'");
            outputWriter.Received(1).WriteLine("{");
            outputWriter.Received(1).WriteLine(".custom instance void [System.Runtime]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78 63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )");
            outputWriter.Received(1).WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = ( 01 00 14 64 6F 74 6E 65 74 2D 69 6C 64 61 73 6D 2E 53 61 6D 70 6C 65 00 00 )");
            outputWriter.Received(1).WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = ( 01 00 13 50 61 63 6B 61 67 65 20 44 65 73 63 72 69 70 74 69 6F 6E 00 00 )");
            outputWriter.Received(1).WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )");
            outputWriter.Received(1).WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyInformationalVersionAttribute::.ctor(string) = ( 01 00 05 31 2E 30 2E 30 00 00 )");
            outputWriter.Received(1).WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyProductAttribute::.ctor(string) = ( 01 00 14 64 6F 74 6E 65 74 2D 69 6C 64 61 73 6D 2E 53 61 6D 70 6C 65 00 00 )");
            outputWriter.Received(1).WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyTitleAttribute::.ctor(string) = ( 01 00 14 64 6F 74 6E 65 74 2D 69 6C 64 61 73 6D 2E 53 61 6D 70 6C 65 00 00 )");
            outputWriter.Received(1).WriteLine(".hash algorithm 0x00008004");
            outputWriter.Received(1).WriteLine(".ver 1:0:0:0");
            outputWriter.Received(1).WriteLine("}");
        }
    }
}