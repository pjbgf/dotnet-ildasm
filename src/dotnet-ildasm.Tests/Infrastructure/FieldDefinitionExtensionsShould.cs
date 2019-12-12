using System.Linq;
using DotNet.Ildasm.Infrastructure;
using DotNet.Ildasm.Tests.Internal;
using Mono.Cecil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class FieldDefinitionExtensionsShould
    {
        [Theory]
        [InlineData("PublicClass", "StaticReadonlyField", ".field public static initonly string StaticReadonlyField")]
        [InlineData("PublicClass", "ReadonlyField", ".field public initonly string ReadonlyField")]
        [InlineData("PublicClass", "Field", ".field public initonly string Field")]
#if NETFRAMEWORK
        [InlineData("PublicClass", "<Property1>k__BackingField", @".field private string '<Property1>k__BackingField'
.custom instance void class [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 )
.custom instance void class [mscorlib]System.Diagnostics.DebuggerBrowsableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggerBrowsableState) = ( 01 00 00 00 00 00 00 00 )")")]
        [InlineData("SomeClassWithAttribute", "SomeFieldWithAttribute", @".field public initonly string SomeFieldWithAttribute
.custom instance void class [mscorlib]System.Diagnostics.DebuggerDisplayAttribute::.ctor(string) = ( 01 00 0B 4C 65 76 65 6C 3D 46 69 65 6C 64 00 00 )")]
#else
        [InlineData("PublicClass", "<Property1>k__BackingField", @".field private string '<Property1>k__BackingField'
.custom instance void class [netstandard]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 )
.custom instance void class [netstandard]System.Diagnostics.DebuggerBrowsableAttribute::.ctor(valuetype [netstandard]System.Diagnostics.DebuggerBrowsableState) = ( 01 00 00 00 00 00 00 00 )")]
        [InlineData("SomeClassWithAttribute", "SomeFieldWithAttribute", @".field public initonly string SomeFieldWithAttribute
.custom instance void class [netstandard]System.Diagnostics.DebuggerDisplayAttribute::.ctor(string) = ( 01 00 0B 4C 65 76 65 6C 3D 46 69 65 6C 64 00 00 )")]
#endif
        public void Write_Method_Signature(string className, string fieldName, string expectedIL)
        {
            var outputWriter = new OutputWriterDouble();
            var type = DataHelper.SampleAssembly.Value.MainModule.Types.FirstOrDefault(x => x.Name == className);
            var method = type.Fields.FirstOrDefault(x => x.Name == fieldName);

            method.WriteIL(outputWriter);

            Assert.Equal(expectedIL, outputWriter.ToString());
        }
    }
}
