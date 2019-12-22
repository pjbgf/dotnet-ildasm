using System.Linq;
using DotNet.Ildasm.Tests.Internal;
using Mono.Cecil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class EventDefinitionExtensionsShould
    {
        private readonly OutputWriterDouble _outputWriter;
        private readonly AssemblyDefinition _assemblyDefinition;
        private readonly IOutputWriter _outputWriterMock;


        public EventDefinitionExtensionsShould()
        {
            _outputWriter = new OutputWriterDouble();
            _assemblyDefinition = DataHelper.SampleAssembly.Value;
            _outputWriterMock = Substitute.For<IOutputWriter>();
        }

        [Theory]
        [InlineData("SomeClassWithAttribute", "SomeEventWithAttribute", ".event class [netstandard]System.EventHandler`1<System.Object> SomeEventWithAttribute")]
        public void Write_Event_Signature(string className, string eventName, string expectedIL)
        {
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == className);
            var eventDefinition = type.Events.FirstOrDefault(x => x.Name == eventName);

            eventDefinition.WriteILSignature(_outputWriter);

            Assert.Equal(expectedIL, _outputWriter.ToString());
        }

        [Fact]
        public void Write_Custom_Attributes()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "SomeClassWithAttribute");
            var eventDefinition = type.Events.First(x => x.Name == "SomeEventWithAttribute");

            eventDefinition.WriteILBody(_outputWriterMock);
            _outputWriterMock.Received(1).WriteLine(Arg.Is<string>(
                x => new string [] {
#if NETFRAMEWORK
                    ".custom instance void class dotnet_ildasm.Sample.Classes.SomeAttribute::.ctor() = ( 01 00 00 00 )",
#else
                    ".custom instance void class dotnet_ildasm.Sample.Classes.SomeAttribute::.ctor() = ( 01 00 00 00 )",
#endif
                }.Contains(x)
            ));
        }

        [Fact]
        public void Write_Event_Methods()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "SomeClassWithAttribute");
            var eventDefinition = type.Events.First(x => x.Name == "SomeEventWithAttribute");

            eventDefinition.WriteILBody(_outputWriterMock);
            _outputWriterMock.Received(2).WriteLine(Arg.Is<string>(
                x => new string [] {
#if NETFRAMEWORK
                    ".addon instance default void dotnet_ildasm.Sample.Classes.SomeClassWithAttribute::add_SomeEventWithAttribute (class [mscorlib]System.EventHandler`1<System.Object> 'value')",
                    ".removeon instance default void dotnet_ildasm.Sample.Classes.SomeClassWithAttribute::remove_SomeEventWithAttribute (class [mscorlib]System.EventHandler`1<System.Object> 'value')"
#else
                    ".addon instance default void dotnet_ildasm.Sample.Classes.SomeClassWithAttribute::add_SomeEventWithAttribute (class [netstandard]System.EventHandler`1<System.Object> 'value')",
                    ".removeon instance default void dotnet_ildasm.Sample.Classes.SomeClassWithAttribute::remove_SomeEventWithAttribute (class [netstandard]System.EventHandler`1<System.Object> 'value')"
#endif
                }.Contains(x)
            ));
        }

        [Fact]
        public void Write_Event_Static_Methods()
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