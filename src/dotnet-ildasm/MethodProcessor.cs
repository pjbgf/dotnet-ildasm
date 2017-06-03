using System.Linq;
using System.Text;
using DotNet.Ildasm.Infrastructure;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public class MethodProcessor
    {
        private readonly IOutputWriter _outputWriter;

        public MethodProcessor(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void WriteBody(MethodDefinition method)
        {
            _outputWriter.WriteLine("{");

            if (method.DeclaringType.Module.EntryPoint == method)
                _outputWriter.WriteLine(".entrypoint");

            if (method.HasBody)
            {
                var @params = method.Parameters.Where(x => x.HasCustomAttributes).ToArray();
                for (int i = 0; i < @params.Length; i++)
                {
                    _outputWriter.WriteLine($".param [{i + 1}]"); // 1-based array?
                    _outputWriter.WriteLine(@params[0].CustomAttributes.First().ToIL());
                }

                _outputWriter.WriteLine($"// Code size {method.Body.CodeSize}");
                _outputWriter.WriteLine($".maxstack {method.Body.MaxStackSize}");

                WriteLocalVariablesIfNeeded(method);

                var ilProcessor = method.Body.GetILProcessor();
                foreach (var instruction in ilProcessor.Body.Instructions)
                {
                    instruction.WriteIL(_outputWriter);
                }
            }

            _outputWriter.WriteLine($"}}// End of method {method.FullName}");
        }

        private void WriteLocalVariablesIfNeeded(MethodDefinition method)
        {
            if (method.Body.InitLocals)
            {
                string variables = string.Empty;
                int i = 0;

                foreach (var variable in method.Body.Variables)
                {
                    if (i > 0)
                        variables += ", ";

                    variables += $"{variable.VariableType.ToIL()} V_{i++}";
                }

                _outputWriter.WriteLine($".locals init({variables})");
            }
        }
    }
}