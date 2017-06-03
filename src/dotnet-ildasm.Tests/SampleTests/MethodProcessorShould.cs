using System.Linq;
using DotNet.Ildasm.Tests.Internal;
using Mono.Cecil;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.SampleTests
{
    public class MethodProcessorShould
    {
        private readonly IOutputWriter _outputWriterMock;

        public MethodProcessorShould()
        {
            _outputWriterMock = Substitute.For<IOutputWriter>();
        }

        [Fact]
        public void Be_Able_To_Support_Params_Keyword()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methodDefinition = type.Methods.First(x => x.Name == "PublicVoidMethodParams");
            var methodProcessor = new MethodProcessor(_outputWriterMock);

            methodProcessor.WriteBody(methodDefinition);

            _outputWriterMock.Received(1).WriteLine(".param [1]");
            _outputWriterMock.Received(1).WriteLine(".custom instance void [System.Runtime]System.ParamArrayAttribute::.ctor() = ( 01 00 00 00 )");
        }

        [Fact]
        public void Be_Able_To_Initialise_Locals()
        {
            var type = DataHelper.SampleAssembly.Value.Modules.First().Types.First(x => x.Name == "PublicClass");
            var methodDefinition = type.Methods.First(x => x.Name == "UsingIF");
            var methodProcessor = new MethodProcessor(_outputWriterMock);

            methodProcessor.WriteBody(methodDefinition);

            _outputWriterMock.Received(1).WriteLine(".locals init(int32 V_0, boolean V_1)");
        }
    }
}