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
        [InlineData("PublicClass", "<Property1>k__BackingField", ".field private string '<Property1>k__BackingField'")]
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
