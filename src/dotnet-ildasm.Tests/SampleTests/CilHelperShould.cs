using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Mono.Cecil;
using Xunit;

namespace DotNet.Ildasm.SampleTests
{
    public class CilHelperShould
    {
        private readonly CilHelper _cilHelper;
        private readonly AssemblyDefinition _assemblyDefinition;
        private static readonly string DotnetIldasmSampleStandardDll = "dotnet-ildasm.Sample.Standard.dll";

        public CilHelperShould()
        {
            _cilHelper = new CilHelper();
            _assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly(DotnetIldasmSampleStandardDll);
        }

        [Theory]
        [InlineData("PrivateClass", ".class private auto ansi beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PrivateClass extends [System.Runtime]System.Object")]
        [InlineData("PublicClass", ".class public auto ansi beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PublicClass extends [System.Runtime]System.Object")]
        [InlineData("PublicSealedClass", ".class public auto ansi sealed beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PublicSealedClass extends [System.Runtime]System.Object")]
        [InlineData("PublicAbstractClass", ".class public abstract auto ansi beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PublicAbstractClass extends [System.Runtime]System.Object")]
        [InlineData("DerivedPublicClass", ".class public auto ansi beforefieldinit dotnet_ildasm.Sample.Standard.Classes.DerivedPublicClass extends dotnet_ildasm.Sample.Standard.Classes.PublicAbstractClass")]
        public void Generate_Valid_IL_For_Class_Signatures(string className, string expectedIL)
        {
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == className);
            
            var signature = _cilHelper.GetTypeSignature(type);

            Assert.Equal(expectedIL, signature);
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
        public void Generate_Valid_IL_For_Method_Signatures(string className, string methodName, string expectedIL)
        {
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == className);
            var method = type.Methods.FirstOrDefault(x => x.Name == methodName);
            
            var signature = _cilHelper.GetMethodSignature(method);

            Assert.Equal(expectedIL, signature);
        }

        [Fact]
        public void Extract_ImageBase_Directive()
        {
            var expected = ".imagebase 0x10000000";
            
            var peHeader = _cilHelper.GetPeHeader(DotnetIldasmSampleStandardDll);
            var actual = _cilHelper.GetImageBaseDirective(peHeader);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Extract_FileAlignment_Directive()
        {
            var expected = ".file alignment 0x00000200";
            
            var peHeader = _cilHelper.GetPeHeader(DotnetIldasmSampleStandardDll);
            var actual = _cilHelper.GetFileAlignmentDirective(peHeader);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Extract_StackReserve_Directive()
        {
            var expected = ".stackreserve 0x00100000";
            
            var peHeader = _cilHelper.GetPeHeader(DotnetIldasmSampleStandardDll);
            var actual = _cilHelper.GetStackReserveDirective(peHeader);

            Assert.Equal(expected, actual);
        }
    }
}