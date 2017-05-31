using System;
using System.IO;
using System.Reflection.PortableExecutable;
using DotNet.Ildasm.Infrastructure;

namespace DotNet.Ildasm
{
    public class ModuleDirectivesProcessor : IModuleDirectivesProcessor
    {
        private readonly IOutputWriter _outputWriter;
        private readonly PEHeaders _peHeaders;
        private readonly string _moduleName;

        public ModuleDirectivesProcessor(string assemblyPath, IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
            using (var stream = File.OpenRead(assemblyPath))
            using (var peFile = new PEReader(stream))
            {
                _moduleName = Path.GetFileName(assemblyPath);
                _peHeaders = peFile.PEHeaders;
            }
        }

        public void WriteModuleDirective()
        {
            _outputWriter.WriteLine($".module '{_moduleName}'");
        }

        public void WriteModuleVersionId(Guid moduleVersionId)
        {
            _outputWriter.WriteLine($"// MVID: {{{moduleVersionId}}}");
        }
        
        public void WriteImageBaseDirective()
        {
            _outputWriter.WriteLine($".imagebase {_peHeaders.PEHeader.ImageBase.ToHexadecimal()}");
        }

        public void WriteFileAlignmentDirective()
        {
            _outputWriter.WriteLine($".file alignment {_peHeaders.PEHeader.FileAlignment.ToHexadecimal()}");
        }

        public void WriteStackReserveDirective()
        {
            _outputWriter.WriteLine($".stackreserve {_peHeaders.PEHeader.SizeOfStackReserve.ToHexadecimal()}");
        }

        public void WriteSubsystemDirective()
        {
            _outputWriter.WriteLine($".subsystem {Convert.ToUInt16(_peHeaders.PEHeader.Subsystem).ToHexadecimal()}  // {Enum.GetName(typeof(Subsystem), _peHeaders.PEHeader.Subsystem)}");
        }

        public void WriteCornFlagsDirective()
        {
            _outputWriter.WriteLine($".corflags {Convert.ToInt32(_peHeaders.CorHeader.Flags).ToHexadecimal()}  // {Enum.GetName(typeof(CorFlags), _peHeaders.CorHeader.Flags)}");
        }
    }
}