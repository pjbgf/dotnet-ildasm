namespace DotNet.Ildasm
{
    public class ExecutionResult
    {
        public ExecutionResult(bool succeeded, string message = "")
        {
            Succeeded = succeeded;
            Message = message;
        }

        public bool Succeeded { get; }

        public string Message { get; }
    }
}