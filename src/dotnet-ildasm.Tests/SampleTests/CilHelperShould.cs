using System;
using System.Linq;
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
    }
}