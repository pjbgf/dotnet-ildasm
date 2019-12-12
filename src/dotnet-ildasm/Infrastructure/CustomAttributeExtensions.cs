using Mono.Cecil;
using System;
using System.Linq;

namespace DotNet.Ildasm.Infrastructure
{
    public static class CustomAttributeExtensions
    {
        public static void WriteIL(this CustomAttribute customAttribute, IOutputWriter outputWriter)
        {
            outputWriter.WriteLine($".custom instance void class {customAttribute.AttributeType.ToIL()}::{customAttribute.Constructor.Name}{GetConstructorArguments(customAttribute)}");
        }

        private static string GetConstructorArguments(CustomAttribute customAttribute)
        {
            CustomAttributeArgument? argument = null;

            try
            {
                argument = customAttribute.ConstructorArguments.FirstOrDefault();
            }
            catch
            {
            }

            return $"({((argument == null || !customAttribute.HasConstructorArguments) ? "" : argument.Value.Type.ToIL())})" +
                   $" = ( {BitConverter.ToString(customAttribute.GetBlob()).Replace("-", " ")} )";
        }
    }
}
