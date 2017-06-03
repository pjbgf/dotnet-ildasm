using System.Linq;
using DotNet.Ildasm.Tests.Internal;
using Mono.Cecil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.SampleTests
{
    public class MethodProcessorShould
    {
        private readonly IOutputWriter _outputWriterMock;
        private readonly AssemblyDefinition _assemblyDefinition;

        public MethodProcessorShould()
        {
            _outputWriterMock = Substitute.For<IOutputWriter>();
            _assemblyDefinition = DataHelper.SampleAssembly.Value;
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
            var methodProcessor = new MethodProcessor(_outputWriterMock);
            
            methodProcessor.WriteSignature(method);

            _outputWriterMock.Received(1).WriteLine(expectedIL);
        }

        [Fact]
        public void Be_Able_To_Support_Params_Keyword()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methodDefinition = type.Methods.First(x => x.Name == "PublicVoidMethodParams");
            var methodProcessor = new MethodProcessor(_outputWriterMock);

            methodProcessor.WriteBody(methodDefinition);

            _outputWriterMock.Received(1).WriteLine(".param [1]");
            _outputWriterMock.Received(1).WriteLine(".custom instance void [System.Runtime]System.ParamArrayAttribute::.ctor() = ( 01 00 00 00 )");
        }

        [Fact]
        public void Be_Able_To_Initialise_Locals()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methodDefinition = type.Methods.First(x => x.Name == "UsingIF");
            var methodProcessor = new MethodProcessor(_outputWriterMock);

            methodProcessor.WriteBody(methodDefinition);

            _outputWriterMock.Received(1).WriteLine(".locals init(int32 V_0, boolean V_1)");
        }
    }
}