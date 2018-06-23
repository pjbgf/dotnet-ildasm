using DotNet.Ildasm.Configuration;

namespace DotNet.Ildasm
{
    public interface IDisassemblerFactory
    {
        Disassembler Create(CommandArgument argument);
    }
}