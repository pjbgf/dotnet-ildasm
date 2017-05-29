using System;
using NSubstitute;
using Xunit;

namespace DotNet.Ildasm.Tests.SampleTests
{
    public class ModuleDirectivesProcessorShould
    {
        private static readonly string DotnetIldasmSampleStandardDll = "dotnet-ildasm.Sample.dll";
        private readonly IOutputWriter _outputWriterMock;

        public ModuleDirectivesProcessorShould()
        {
            _outputWriterMock = Substitute.For<IOutputWriter>();
        }

        [Fact]
        public void Write_ImageBase_Directive()
        {
            var expected = ".imagebase 0x10000000";
            var directivesProcessor = new ModuleDirectivesProcessor(DotnetIldasmSampleStandardDll, _outputWriterMock);
            
            directivesProcessor.WriteImageBaseDirective();

            _outputWriterMock.Received(1).WriteLine(expected);
        }

        [Fact]
        public void Write_ModuleVersionId_Directive()
        {
            var guid = Guid.NewGuid();
            var expected = $"// MVID: {{{guid}}}";
            var directivesProcessor = new ModuleDirectivesProcessor(DotnetIldasmSampleStandardDll, _outputWriterMock);
            
            directivesProcessor.WriteModuleVersionId(guid);

            _outputWriterMock.Received(1).WriteLine(expected);
        }

        [Fact]
        public void Write_FileAlignment_Directive()
        {
            var expected = ".file alignment 0x00000200";
            var directivesProcessor = new ModuleDirectivesProcessor(DotnetIldasmSampleStandardDll, _outputWriterMock);
            
            directivesProcessor.WriteFileAlignmentDirective();

            _outputWriterMock.Received(1).WriteLine(expected);
        }

        [Fact]
        public void Write_StackReserve_Directive()
        {
            var expected = ".stackreserve 0x00100000";
            var directivesProcessor = new ModuleDirectivesProcessor(DotnetIldasmSampleStandardDll, _outputWriterMock);
            
            directivesProcessor.WriteStackReserveDirective();

            _outputWriterMock.Received(1).WriteLine(expected);
        }

        [Fact]
        public void Write_Subsystem_Directive()
        {
            var expected = ".subsystem 0x0003  // WindowsCui";
            var directivesProcessor = new ModuleDirectivesProcessor(DotnetIldasmSampleStandardDll, _outputWriterMock);
            
            directivesProcessor.WriteSubsystemDirective();

            _outputWriterMock.Received(1).WriteLine(expected);
        }

        [Fact]
        public void Write_CornFlags_Directive()
        {
            var expected = ".corflags 0x00000001  // ILOnly";
            var directivesProcessor = new ModuleDirectivesProcessor(DotnetIldasmSampleStandardDll, _outputWriterMock);
            
            directivesProcessor.WriteCornFlagsDirective();

            _outputWriterMock.Received(1).WriteLine(expected);
        }
    }
}