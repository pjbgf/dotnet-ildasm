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
        public void Generate_Valid_IL_PrivateClass_Signature()
        {
            var expectedPublicClassIL =
                ".class private auto ansi beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PrivateClass extends [System.Runtime]System.Object";
            var publicClass = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == "PrivateClass");
            
            var signature = _cilHelper.GetTypeSignature(publicClass);

            Assert.Equal(expectedPublicClassIL, signature);
        }

        [Fact]
        public void Generate_Valid_IL_PublicClass_Signature()
        {
            var expectedPublicClassIL =
                ".class public auto ansi beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PublicClass extends [System.Runtime]System.Object";
            var publicClass = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == "PublicClass");
            
            var signature = _cilHelper.GetTypeSignature(publicClass);

            Assert.Equal(expectedPublicClassIL, signature);
        }

        [Fact]
        public void Generate_Valid_IL_PublicSealedClass_Signature()
        {
            var expectedPublicClassIL =
                ".class public auto ansi sealed beforefieldinit dotnet_ildasm.Sample.Standard.Classes.PublicSealedClass extends [System.Runtime]System.Object";
            var publicClass = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == "PublicSealedClass");
            
            var signature = _cilHelper.GetTypeSignature(publicClass);

            Assert.Equal(expectedPublicClassIL, signature);
        }
    }
}