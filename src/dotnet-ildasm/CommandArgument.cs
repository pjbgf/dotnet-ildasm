namespace DotNet.Ildasm
{
    public class CommandArgument
    {
        public string Assembly { get; set; }
        public string OutputFile { get; set; }
        public string Item { get; set; }
        public bool ForceOverwrite { get; set; }
        
        public bool HasOutputPathSet => OutputFile?.Length > 0;
    }
}