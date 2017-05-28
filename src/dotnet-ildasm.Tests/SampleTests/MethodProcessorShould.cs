using System.Linq;
using Mono.Cecil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.SampleTests
{
    public class MethodProcessorShould
    {
        private static readonly string DotnetIldasmSampleStandardDll = "dotnet-ildasm.Sample.dll";
        private readonly IOutputWriter _outputWriterMock;
        private readonly AssemblyDefinition _assemblyDefinition;

        public MethodProcessorShould()
        {
            _outputWriterMock = Substitute.For<IOutputWriter>();
            _assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(DotnetIldasmSampleStandardDll);
        }

        [Theory]
        [InlineData("PublicClass", "PublicVoidMethod", ".method public hidebysig instance void PublicVoidMethod() cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodSingleParameter", ".method public hidebysig instance void PublicVoidMethodSingleParameter(string parameter1) cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodTwoParameters", ".method public hidebysig instance void PublicVoidMethodTwoParameters(string parameter1, int32 parameter2) cil managed")]
        [InlineData("PublicClass", "PublicVoidMethodParams", ".method public hidebysig instance void PublicVoidMethodParams(string[] parameters) cil managed", Skip = "Add support to params key word bug. #13")]
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