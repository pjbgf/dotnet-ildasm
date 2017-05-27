using System;
using System.IO;
using System.Reflection.PortableExecutable;

namespace DotNet.Ildasm
{
    public static class PeHeaderHelper
    {
        public static PEHeaders GetPeHeaders(string assemblyPath)
        {
            using (var stream = File.OpenRead(assemblyPath))
            using (var peFile = new PEReader(stream))
            {
                return peFile.PEHeaders;
            }
        }

        public static string GetImageBaseDirective(PEHeader peHeader)
        {
            return $".imagebase {peHeader.ImageBase.ToHexadecimal()}";
        }

        public static string GetFileAlignmentDirective(PEHeader peHeader)
        {
            return $".file alignment {peHeader.FileAlignment.ToHexadecimal()}";
        }

        public static string GetStackReserveDirective(PEHeader peHeader)
        {
            return $".stackreserve {peHeader.SizeOfStackReserve.ToHexadecimal()}";
        }

        public static string GetSubsystemDirective(PEHeader peHeader)
        {
            return $".subsystem {Convert.ToUInt16(peHeader.Subsystem).ToHexadecimal()}  // {Enum.GetName(typeof(Subsystem), peHeader.Subsystem)}";
        }

        public static string GetCornFlagsDirective(PEHeaders peHeaders)
        {
            return $".corflags {Convert.ToInt32(peHeaders.CorHeader.Flags).ToHexadecimal()}  // {Enum.GetName(typeof(CorFlags), peHeaders.CorHeader.Flags)}";
        }
    }
}
