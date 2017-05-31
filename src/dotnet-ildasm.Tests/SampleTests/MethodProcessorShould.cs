using System.Linq;
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
        [InlineData("PublicClass", "PublicVoidMethodTwoParameters", ".method public hidebysig instance void PublicVoidMethodTwoParameters(string parameter1, [System.Runtime]System.Int32 parameter2) cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodParams", ".method public hidebysig instance void PublicVoidMethodParams(string[] parameters) cil managed")]
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
    }
}