using System;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace DotNet.Ildasm
{
    public class CilHelper
    {
        public string GetTypeSignature(TypeDefinition typeDefinition)
        {
            var builder = new StringBuilder();

            builder.Append(".class");

            if (typeDefinition.IsPublic)
                builder.Append(" public");
            else
                builder.Append(" private");

            if (typeDefinition.IsSequentialLayout)
                builder.Append(" sequential");

            if (typeDefinition.IsInterface)
                builder.Append(" interface");

            if (typeDefinition.IsAbstract)
                builder.Append(" abstract");

            if (typeDefinition.IsAutoLayout)
                builder.Append(" auto");

            if (typeDefinition.IsAnsiClass)
                builder.Append(" ansi");

            if (typeDefinition.IsSealed)
                builder.Append(" sealed");

            if (typeDefinition.IsBeforeFieldInit)
                builder.Append(" beforefieldinit");

            builder.Append($" {typeDefinition.FullName}");

            if (typeDefinition.BaseType != null)
                builder.Append(
                    $" extends {GetFullTypeName(typeDefinition.BaseType)}");

            if (typeDefinition.HasInterfaces)
                builder.Append(
                    $" implements {string.Join(", ", typeDefinition.Interfaces.Select(x => x.InterfaceType.FullName))}");

            return builder.ToString();
        }

        public string GetFullTypeName(TypeReference typeReference)
        {
            if (string.Compare(typeReference.Scope.Name, typeReference.Module.Name,
                    StringComparison.CurrentCultureIgnoreCase) == 0)
                return $"{typeReference.FullName}";

            return $"[{typeReference.Scope.Name}]{typeReference.FullName}";
        }

        

        public string GetCustomAttribute(CustomAttribute customAttribute)
        {
            return $".custom instance void {GetFullTypeName(customAttribute.AttributeType)}::{customAttribute.Constructor.Name}" +
                   $"{GetConstructorArguments(customAttribute)}";
        }

        private string GetConstructorArguments(CustomAttribute customAttribute)
        {
            StringBuilder builder = new StringBuilder();

            var argument = customAttribute.ConstructorArguments.FirstOrDefault();

            if (!customAttribute.HasConstructorArguments)
                builder.Append("()");
            else
                builder.Append($"({argument.Type.MetadataType.ToString().ToLowerInvariant()})");
            
            builder.Append($" = ( {BitConverter.ToString(customAttribute.GetBlob()).Replace("-", " ")} )");

            return builder.ToString();
        }
    }
}