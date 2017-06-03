using DotNet.Ildasm.Infrastructure;
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

            Received.InOrder(() =>
            {
                outputWriter.WriteLine(".assembly 'dotnet-ildasm.Sample'");
                outputWriter.WriteLine("{");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 )");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78 63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Runtime.Versioning.TargetFrameworkAttribute::.ctor(string) = ( 01 00 19 2E 4E 45 54 53 74 61 6E 64 61 72 64 2C 56 65 72 73 69 6F 6E 3D 76 31 2E 36 01 00 54 0E 14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79 4E 61 6D 65 00 )");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = ( 01 00 14 64 6F 74 6E 65 74 2D 69 6C 64 61 73 6D 2E 53 61 6D 70 6C 65 00 00 )");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = ( 01 00 05 44 65 62 75 67 00 00 )");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = ( 01 00 13 50 61 63 6B 61 67 65 20 44 65 73 63 72 69 70 74 69 6F 6E 00 00 )");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyInformationalVersionAttribute::.ctor(string) = ( 01 00 05 31 2E 30 2E 30 00 00 )");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyProductAttribute::.ctor(string) = ( 01 00 14 64 6F 74 6E 65 74 2D 69 6C 64 61 73 6D 2E 53 61 6D 70 6C 65 00 00 )");
                outputWriter.WriteLine(".custom instance void [System.Runtime]System.Reflection.AssemblyTitleAttribute::.ctor(string) = ( 01 00 14 64 6F 74 6E 65 74 2D 69 6C 64 61 73 6D 2E 53 61 6D 70 6C 65 00 00 )");
                outputWriter.WriteLine(".hash algorithm 0x00008004");
                outputWriter.WriteLine(".ver 1:0:0:0");
                outputWriter.WriteLine("}");
            });
        }
    }
}