using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Xunit;

namespace DotNet.Ildasm.SampleTests
{
    public class CilHelperShould
    {
        private readonly CilHelper _cilHelper;
        private readonly AssemblyDefinition _assemblyDefinition;

        public CilHelperShould()
        {
            _cilHelper = new CilHelper();
            _assemblyDefinition = Mono.Cecil.AssemblyDefinition.ReadAssembly("dotnet-ildasm.Sample.Standard.dll");
        }

        [Fact]
        public void Generate_Valid_IL_For_PrivateClass_Signature()
        {
            var expectedIL =
                ".class private auto ansi beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PrivateClass extends [System.Runtime]System.Object";
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == "PrivateClass");
            
            var signature = _cilHelper.GetTypeSignature(type);

            Assert.Equal(expectedIL, signature);
        }

        [Fact]
        public void Generate_Valid_IL_For_PublicClass_Signature()
        {
            var expectedIL =
                ".class public auto ansi beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PublicClass extends [System.Runtime]System.Object";
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == "PublicClass");
            
            var signature = _cilHelper.GetTypeSignature(type);

            Assert.Equal(expectedIL, signature);
        }

        [Fact]
        public void Generate_Valid_IL_For_PublicSealedClass_Signature()
        {
            var expectedIL =
                ".class public auto ansi sealed beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PublicSealedClass extends [System.Runtime]System.Object";
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == "PublicSealedClass");
            
            var signature = _cilHelper.GetTypeSignature(type);

            Assert.Equal(expectedIL, signature);
        }
        

        [Fact]
        public void Generate_Valid_IL_For_PublicVoidMethod_Signature()
        {
            var expectedIL =
                ".method public hidebysig instance void PublicVoidMethod() cil managed";
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == "PublicClass");
            var method = type.Methods.FirstOrDefault(x => x.Name == "PublicVoidMethod");
            
            var signature = _cilHelper.GetMethodSignature(method);

            Assert.Equal(expectedIL, signature);
        }
    }
}