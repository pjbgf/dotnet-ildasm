using System;
using Mono.Cecil;

namespace DotNet.Ildasm.Infrastructure
{
    public static class TypeReferenceExtensions
    {
        public static string ToILType(this TypeReference typeReference)
        {
            if (typeReference.MetadataType == MetadataType.Void)
                return "void";
            if (typeReference.MetadataType == MetadataType.String)
                return "string";
            if (typeReference.MetadataType == MetadataType.Array)
                return $"{typeReference.GetElementType().ToILType()}[]";

            if (string.Compare(typeReference.Scope.Name, typeReference.Module.Name,
                    StringComparison.CurrentCultureIgnoreCase) == 0)
                return $"{typeReference.FullName}";

            return $"[{typeReference.Scope.Name}]{typeReference.FullName}";
        }
    }
}
