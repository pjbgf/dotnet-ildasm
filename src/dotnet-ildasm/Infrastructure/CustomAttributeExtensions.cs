using Mono.Cecil;
using System;
using System.Linq;

namespace DotNet.Ildasm.Infrastructure
{
    public static class CustomAttributeExtensions
    {
        public static string ToIL(this CustomAttribute customAttribute)
        {
            return $".custom instance void {customAttribute.AttributeType.ToIL()}::{customAttribute.Constructor.Name}{GetConstructorArguments(customAttribute)}";
        }

        private static string GetConstructorArguments(CustomAttribute customAttribute)
        {
            var argument = customAttribute.ConstructorArguments.FirstOrDefault();

            return $"({(!customAttribute.HasConstructorArguments ? "" : argument.Type.ToIL())})" +
                   $" = ( {BitConverter.ToString(customAttribute.GetBlob()).Replace("-", " ")} )";
        }
    }
}
