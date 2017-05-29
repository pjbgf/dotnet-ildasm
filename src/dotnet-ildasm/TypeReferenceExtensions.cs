using System;
using Mono.Cecil;

namespace DotNet.Ildasm
{
    public static class TypeReferenceExtensions
    {
        public static string ToILType(this TypeReference typeReference)
        {
            if (string.Compare(typeReference.Scope.Name, typeReference.Module.Name,
                    StringComparison.CurrentCultureIgnoreCase) == 0)
                return $"{typeReference.FullName}";

            return $"[{typeReference.Scope.Name}]{typeReference.FullName}";
        }
    }
}
