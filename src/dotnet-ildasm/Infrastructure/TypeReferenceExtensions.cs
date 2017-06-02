using System;
using Mono.Cecil;

namespace DotNet.Ildasm.Infrastructure
{
    public static class TypeReferenceExtensions
    {
        public static string ToILNameFormatt(this TypeReference typeReference)
        {
            if (string.Compare(typeReference.Scope.Name, typeReference.Module.Name,
                    StringComparison.CurrentCultureIgnoreCase) == 0)
                return $"{typeReference.FullName}";

            return $"[{typeReference.Scope.Name}]{typeReference.FullName}";
        }

        public static string ToILType(this TypeReference typeReference)
        {
            if(typeReference.MetadataType == MetadataType.ValueType || typeReference.MetadataType == MetadataType.Class)
                return $"{typeReference.MetadataType.ToString().ToLowerInvariant()} {ToILNameFormatt(typeReference)}";

            if (typeReference.MetadataType != MetadataType.Array)
                return typeReference.MetadataType.ToString().ToLowerInvariant();

            if (typeReference.MetadataType == MetadataType.Array)
                return $"{typeReference.GetElementType().ToILType()}[]";

            throw new NotSupportedException();
        }
    }
}
