using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotNet.Ildasm.Interop
{
    public static class BinarySerializer
    {
        public static byte[] Serialize(object currentState)
        {
            if (currentState == null)
                return null;

            var formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, currentState);

            return stream.ToArray();
        }
    }
}
