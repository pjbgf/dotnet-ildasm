using System;
using Mono.Cecil;

namespace DotNet.Ildasm.Infrastructure
{
    public static class TypeReferenceExtensions
    {
        public static string ToPrefixedTypeName(this TypeReference typeReference)
        {
            if (string.Compare(typeReference.Scope.Name, typeReference.Module.Name,
                    StringComparison.CurrentCultureIgnoreCase) == 0)
                return $"{typeReference.FullName}";

            return $"[{typeReference.Scope.Name}]{typeReference.FullName}";
        }

        public static string ToIL(this TypeReference typeReference)
        {
            if(typeReference.MetadataType == MetadataType.ValueType || typeReference.MetadataType == MetadataType.Class)
                return $"{ToPrefixedTypeName(typeReference)}";

            if (typeReference.MetadataType != MetadataType.Array)
                return typeReference.MetadataType.ToString().ToLowerInvariant();

            if (typeReference.MetadataType == MetadataType.Array)
                return $"{typeReference.GetElementType().ToIL()}[]";

            throw new NotSupportedException();
        }
    }
}
