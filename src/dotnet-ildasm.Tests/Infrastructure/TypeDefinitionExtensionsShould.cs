using System.Linq;
using DotNet.Ildasm.Infrastructure;
using DotNet.Ildasm.Tests.Internal;
using Mono.Cecil;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class TypeDefinitionExtensions
    {
        private readonly AssemblyDefinition _assemblyDefinition;
        private readonly OutputWriterDouble _outputWriter;

        public TypeDefinitionExtensions()
        {
            _assemblyDefinition = DataHelper.SampleAssembly.Value;
            _outputWriter = new OutputWriterDouble();
        }

        [Theory]
#if NETFRAMEWORK
        [InlineData("PrivateClass",
            ".class private auto ansi beforefieldinit dotnet_ildasm.Sample.Classes.PrivateClass extends [System.Runtime]System.Object")]
        [InlineData("PublicClass",
            ".class public auto ansi beforefieldinit dotnet_ildasm.Sample.Classes.PublicClass extends [System.Runtime]System.Object")]
        [InlineData("PublicSealedClass",
            ".class public auto ansi sealed beforefieldinit dotnet_ildasm.Sample.Classes.PublicSealedClass extends [System.Runtime]System.Object")]
        [InlineData("PublicAbstractClass",
            ".class public abstract auto ansi beforefieldinit dotnet_ildasm.Sample.Classes.PublicAbstractClass extends [System.Runtime]System.Object")]
 #else
        [InlineData("PrivateClass",
            ".class private auto ansi beforefieldinit dotnet_ildasm.Sample.Classes.PrivateClass extends [netstandard]System.Object")]
        [InlineData("PublicClass",
            ".class public auto ansi beforefieldinit dotnet_ildasm.Sample.Classes.PublicClass extends [netstandard]System.Object")]
        [InlineData("PublicSealedClass",
            ".class public auto ansi sealed beforefieldinit dotnet_ildasm.Sample.Classes.PublicSealedClass extends [netstandard]System.Object")]
        [InlineData("PublicAbstractClass",
            ".class public abstract auto ansi beforefieldinit dotnet_ildasm.Sample.Classes.PublicAbstractClass extends [netstandard]System.Object")]
 #endif
        [InlineData("DerivedPublicClass",
            ".class public auto ansi beforefieldinit dotnet_ildasm.Sample.Classes.DerivedPublicClass extends dotnet_ildasm.Sample.Classes.PublicAbstractClass")]
        public void Generate_Valid_IL_For_Class_Signatures(string className, string expectedIL)
        {
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == className);
            
            type.WriteILSignature(_outputWriter);
            var actualIL = _outputWriter.ToString();

            Assert.Equal(expectedIL, actualIL);
        }

        [Theory]
#if NETFRAMEWORK
        [InlineData("ParentClass", "NestedClass",
            ".class nested public auto ansi beforefieldinit NestedClass extends [System.Runtime]System.Object")]
#else
        [InlineData("ParentClass", "NestedClass",
            ".class nested public auto ansi beforefieldinit NestedClass extends [netstandard]System.Object")]
#endif
        public void Generate_Valid_IL_For_NestedClasses_Signatures(string parentClassName, string nestedClassName, string expectedIL)
        {
            var parentType = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == parentClassName);
            var nestedType = parentType.NestedTypes.FirstOrDefault(x => x.Name == nestedClassName);
    
            nestedType.WriteILSignature(_outputWriter);
            var actualIL = _outputWriter.ToString();

            Assert.Equal(expectedIL, actualIL);
        }

        [Theory]
#if NETFRAMEWORK
        [InlineData("PublicStruct",
            ".class public sequential ansi sealed beforefieldinit dotnet_ildasm.Sample.Structs.PublicStruct extends [System.Runtime]System.ValueType")]
#else
        [InlineData("PublicStruct",
            ".class public sequential ansi sealed beforefieldinit dotnet_ildasm.Sample.Structs.PublicStruct extends [netstandard]System.ValueType")]
#endif
        public void Generate_Valid_IL_For_Struct_Signatures(string structName, string expectedIL)
        {
            var type = _assemblyDefinition.MainModule.Types.FirstOrDefault(x => x.Name == structName);

            type.WriteILSignature(_outputWriter);
            var actualIL = _outputWriter.ToString();

            Assert.Equal(expectedIL, actualIL);
        }
    }
}