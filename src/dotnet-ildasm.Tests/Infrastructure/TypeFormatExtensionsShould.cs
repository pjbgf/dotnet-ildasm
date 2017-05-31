using System.Linq;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;
using Xunit;

namespace DotNet.Ildasm.Tests.Infrastructure
{
    public class TypeFormatExtensionsShould
    {
        [Fact]
        public void Return_IL_Type_For_Void()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "Program");
            var methoDefinition = type.Methods.First(x => x.Name == "Main");

            var actual = methoDefinition.ReturnType.ToILType();

            Assert.Equal("void", actual);
        }
    }
}
