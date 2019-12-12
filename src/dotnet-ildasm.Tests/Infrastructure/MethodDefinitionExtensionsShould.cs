using System.Linq;
using DotNet.Ildasm.Tests.Internal;
using Mono.Cecil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class MethodDefinitionExtensionsShould
    {
        private readonly OutputWriterDouble _outputWriter;
        private readonly AssemblyDefinition _assemblyDefinition;
        private readonly IOutputWriter _outputWriterMock;


        public MethodDefinitionExtensionsShould()
        {
            _outputWriter = new OutputWriterDouble();
            _assemblyDefinition = DataHelper.SampleAssembly.Value;
            _outputWriterMock = Substitute.For<IOutputWriter>();
        }

        [Theory]
        [InlineData("PublicClass", "PublicVoidMethod", ".method public hidebysig instance void PublicVoidMethod() cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodSingleParameter", ".method public hidebysig instance void PublicVoidMethodSingleParameter(string parameter1) cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodTwoParameters", ".method public hidebysig instance void PublicVoidMethodTwoParameters(string parameter1, int32 parameter2) cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodParams", ".method public hidebysig instance void PublicVoidMethodParams(string[] parameters) cil managed")]
        [InlineData("PublicClass", "set_Property1", ".method public hidebysig specialname instance void set_Property1(string 'value') cil managed")]
        [InlineData("PublicAbstractClass", "PublicAbstractMethod", ".method public hidebysig newslot abstract virtual instance void PublicAbstractMethod() cil managed")]
        [InlineData("PublicAbstractClass", "PublicImplementedMethod", ".method public hidebysig instance void PublicImplementedMethod() cil managed")]
        [InlineData("DerivedPublicClass", "PublicAbstractMethod", ".method public hidebysig virtual instance void PublicAbstractMethod() cil managed")]
        [InlineData("DerivedPublicClass", "PublicAbstractSealedMethod", ".method public hidebysig virtual final instance void PublicAbstractSealedMethod() cil managed")]
        [InlineData("DerivedPublicClass", "PublicImplementedMethod", ".method public hidebysig instance void PublicImplementedMethod() cil managed")]
        public void Write_Method_Signature(string className, string methodName, string expectedIL)
        {
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == className);
            var method = type.Methods.FirstOrDefault(x => x.Name == methodName);

            method.WriteILSignature(_outputWriter);

            Assert.Equal(expectedIL, _outputWriter.ToString());
        }

        [Fact]
        public void Be_Able_To_Support_Params_Keyword()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methodDefinition = type.Methods.First(x => x.Name == "PublicVoidMethodParams");

            methodDefinition.WriteILBody(_outputWriterMock);

            _outputWriterMock.Received(1).WriteLine(".param [1]");
            _outputWriterMock.Received(1).WriteLine(Arg.Is<string>(
                x => new string [] {
#if NETFRAMEWORK
                    ".custom instance void class [System.Runtime]System.ParamArrayAttribute::.ctor() = ( 01 00 00 00 )",
#else
                    ".custom instance void class [netstandard]System.ParamArrayAttribute::.ctor() = ( 01 00 00 00 )"
#endif
                }.Contains(x)
            ));
        }

        [Fact]
        public void Be_Able_To_Initialise_Locals()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methodDefinition = type.Methods.First(x => x.Name == "UsingIF");

            methodDefinition.WriteILBody(_outputWriterMock);

            _outputWriterMock.Received().WriteLine(Arg.Do((string IL_Code) =>
            {
                // For some reason, depending on platform/compilation the same code may generate two different ILs
                // potentially due to compiler optmisations.
                Assert.True(IL_Code == ".locals init(int32 V_0, boolean V_1)" ||
                            IL_Code == ".locals init(int32 V_0)");
            }));
        }

        [Fact]
        public void Be_Able_To_Support_Try_Catch()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methodDefinition = type.Methods.First(x => x.Name == "UsingTryCatch");

            methodDefinition.WriteILBody(_outputWriterMock);

            _outputWriterMock.Received().WriteLine(Arg.Do((string IL_Code) =>
            {
                // For some reason, depending on platform/compilation the same code may generate two different ILs
                // potentially due to compiler optmisations.
                Assert.True(IL_Code == ".locals init(int32 V_0, boolean V_1)" ||
                            IL_Code == ".locals init(int32 V_0)");
            }));
        }
    }
}