using System;

namespace DotNet.Ildasm
 {  
     public interface IModuleDirectivesProcessor
    {
        void WriteModuleDirective();
        void WriteModuleVersionId(Guid moduleVersionId);
        void WriteImageBaseDirective();
        void WriteFileAlignmentDirective();
        void WriteStackReserveDirective();
        void WriteSubsystemDirective();
        void WriteCornFlagsDirective();
    }
}