using System.Linq;
using DotNet.Ildasm.Tests.Internal;
using Mono.Cecil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class PropertyDefinitionExtensionsShould
    {
        private readonly OutputWriterDouble _outputWriter;
        private readonly AssemblyDefinition _assemblyDefinition;
        private readonly IOutputWriter _outputWriterMock;


        public PropertyDefinitionExtensionsShould()
        {
            _outputWriter = new OutputWriterDouble();
            _assemblyDefinition = DataHelper.SampleAssembly.Value;
            _outputWriterMock = Substitute.For<IOutputWriter>();
        }

        [Theory]
        [InlineData("SomeClassWithAttribute", "SomePropertyWithAttribute", ".property instance string SomePropertyWithAttribute ()")]
        [InlineData("SomeClassWithAttribute", "SomeStaticPropertyWithAttribute", ".property string SomeStaticPropertyWithAttribute ()")]
        public void Write_Property_Signature(string className, string propertyName, string expectedIL)
        {
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == className);
            var property = type.Properties.FirstOrDefault(x => x.Name == propertyName);

            property.WriteILSignature(_outputWriter);

            Assert.Equal(expectedIL, _outputWriter.ToString());
        }

        [Fact]
        public void Write_Custom_Attributes()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "SomeClassWithAttribute");
            var propertyDefinition = type.Properties.First(x => x.Name == "SomePropertyWithAttribute");

            propertyDefinition.WriteILBody(_outputWriterMock);
            _outputWriterMock.Received(2).WriteLine(Arg.Is<string>(
                x => new string [] {
#if NETFRAMEWORK
                    ".custom instance void class dotnet_ildasm.Sample.Classes.SomeAttribute::.ctor() = ( 01 00 00 00 )",
                    ".custom instance void class [mscorlib]System.Diagnostics.DebuggerDisplayAttribute::.ctor(string) = ( 01 00 0E 4C 65 76 65 6C 3D 50 72 6F 70 65 72 74 79 00 00 )"
#else
                    ".custom instance void class dotnet_ildasm.Sample.Classes.SomeAttribute::.ctor() = ( 01 00 00 00 )",
                    ".custom instance void class [netstandard]System.Diagnostics.DebuggerDisplayAttribute::.ctor(string) = ( 01 00 0E 4C 65 76 65 6C 3D 50 72 6F 70 65 72 74 79 00 00 )"
#endif
                }.Contains(x)
            ));
        }

        [Fact]
        public void Write_Property_Methods()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "SomeClassWithAttribute");
            var propertyDefinition = type.Properties.First(x => x.Name == "SomePropertyWithAttribute");

            propertyDefinition.WriteILBody(_outputWriterMock);
            _outputWriterMock.Received(2).WriteLine(Arg.Is<string>(
                x => new string [] {
                    ".get instance default string dotnet_ildasm.Sample.Classes.SomeClassWithAttribute::get_SomePropertyWithAttribute ()", 
		            ".set instance default void dotnet_ildasm.Sample.Classes.SomeClassWithAttribute::set_SomePropertyWithAttribute (string 'value')"
                }.Contains(x)
            ));
        }

        [Fact]
        public void Write_Property_Static_Methods()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "SomeClassWithAttribute");
            var propertyDefinition = type.Properties.First(x => x.Name == "SomeStaticPropertyWithAttribute");

            propertyDefinition.WriteILBody(_outputWriterMock);
            _outputWriterMock.Received(2).WriteLine(Arg.Is<string>(
                x => new string [] {
                    ".get default string dotnet_ildasm.Sample.Classes.SomeClassWithAttribute::get_SomeStaticPropertyWithAttribute ()", 
		            ".set default void dotnet_ildasm.Sample.Classes.SomeClassWithAttribute::set_SomeStaticPropertyWithAttribute (string 'value')"
                }.Contains(x)
            ));
        }
    }
}