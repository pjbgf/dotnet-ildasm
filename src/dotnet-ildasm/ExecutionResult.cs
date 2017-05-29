namespace DotNet.Ildasm
{
    public class ExecutionResult
    {
        public ExecutionResult(bool succeeded, string errorMessage = "")
        {
            Succeeded = succeeded;
            ErrorMessage = errorMessage;
        }

        public bool Succeeded { get; }

        public string ErrorMessage { get; }
    }
}