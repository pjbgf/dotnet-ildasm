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
            return $".imagebase 0x{peHeader.ImageBase.ToHexadecimal()}";
        }

        public static string GetFileAlignmentDirective(PEHeader peHeader)
        {
            return $".file alignment 0x{peHeader.FileAlignment.ToHexadecimal()}";
        }

        public static string GetStackReserveDirective(PEHeader peHeader)
        {
            return $".stackreserve 0x{peHeader.SizeOfStackReserve.ToHexadecimal()}";
        }

        public static string GetSubsystemDirective(PEHeader peHeader)
        {
            return $".subsystem 0x{Convert.ToUInt16(peHeader.Subsystem).ToHexadecimal()}  // {Enum.GetName(typeof(Subsystem), peHeader.Subsystem)}";
        }

        public static string GetCornFlagsDirective(PEHeaders peHeaders)
        {
            return $".corflags 0x{Convert.ToInt32(peHeaders.CorHeader.Flags).ToHexadecimal()}  // {Enum.GetName(typeof(CorFlags), peHeaders.CorHeader.Flags)}";
        }
    }
}
