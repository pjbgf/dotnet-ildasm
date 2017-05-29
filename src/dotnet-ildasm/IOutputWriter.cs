namespace DotNet.Ildasm
{
    public interface IOutputWriter
    {
        void Write(string value);
        void WriteLine(string value = "");
    }
}