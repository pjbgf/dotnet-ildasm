namespace DotNet.Ildasm
{
    internal interface IOutputWriter
    {
        void Write(string value);
        void WriteLine(string value = "");
    }
}