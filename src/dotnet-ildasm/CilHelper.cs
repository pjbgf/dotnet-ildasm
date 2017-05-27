using System;
using System.Linq;
using System.Text;
using Mono.Cecil;

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

        public string GetMethodSignature(MethodDefinition method)
        {
            var builder = new StringBuilder();
            builder.Append(".method");

            if (method.IsPublic)
                builder.Append(" public");
            else if (method.IsPrivate)
                builder.Append(" private");

            if (method.IsHideBySig)
                builder.Append(" hidebysig");

            if (method.IsNewSlot)
                builder.Append(" newslot");

            if (method.IsAbstract)
                builder.Append(" abstract");

            if (method.IsVirtual)
                builder.Append(" virtual");

            if (method.IsSpecialName)
                builder.Append(" specialname");

            if (method.IsRuntimeSpecialName)
                builder.Append(" rtspecialname");

            if (!method.IsStatic)
                builder.Append(" instance");

            builder.Append($" {method.ReturnType.MetadataType.ToString().ToLowerInvariant()}");
            builder.Append($" {method.Name}");

            AppendMethodParameters(method, builder);

            if (method.IsManaged)
                builder.Append(" cil managed");

            return builder.ToString();
        }

        private static void AppendMethodParameters(MethodDefinition method, StringBuilder builder)
        {
            builder.Append("(");

            if (method.HasParameters)
            {
                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    if (i > 0)
                        builder.Append(", ");

                    var parameterDefinition = method.Parameters[i];
                    builder.Append($"{parameterDefinition.ParameterType.MetadataType.ToString().ToLowerInvariant()} ");
//                    if (parameterDefinition.ParameterType.IsPrimitive)
//                    {
//                        builder.Append($"{parameterDefinition.ParameterType.MetadataType.ToString().ToLowerInvariant()} ");
//                    }
//                    else
//                    {
//                        builder.Append($"{parameterDefinition.ParameterType.FullName} ");
//                    }

                    builder.Append(parameterDefinition.Name);
                }
            }

            builder.Append(")");
        }
    }
}