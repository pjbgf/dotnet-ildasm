using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;

namespace DotNet.Ildasm.Interop
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
            return $".imagebase 0x{FormatHelper.GetHexadecimal(peHeader.ImageBase)}";
        }

        public static string GetFileAlignmentDirective(PEHeader peHeader)
        {
            return $".file alignment 0x{FormatHelper.GetHexadecimal(peHeader.FileAlignment)}";
        }

        public static string GetStackReserveDirective(PEHeader peHeader)
        {
            return $".stackreserve 0x{FormatHelper.GetHexadecimal(peHeader.SizeOfStackReserve)}";
        }

        public static string GetSubsystemDirective(PEHeader peHeader)
        {
            return $".subsystem 0x{FormatHelper.GetHexadecimal(Convert.ToUInt16(peHeader.Subsystem))}  // {Enum.GetName(typeof(Subsystem), peHeader.Subsystem)}";
        }

        public static string GetCornFlagsDirective(PEHeaders peHeaders)
        {
            return $".corflags 0x{FormatHelper.GetHexadecimal(Convert.ToInt32(peHeaders.CorHeader.Flags))}  // {Enum.GetName(typeof(CorFlags), peHeaders.CorHeader.Flags)}";
        }
    }
}
