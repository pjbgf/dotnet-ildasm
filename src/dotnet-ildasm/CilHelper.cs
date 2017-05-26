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

            if (typeDefinition.IsAutoLayout)
                builder.Append(" auto");

            if (typeDefinition.IsAbstract)
                builder.Append(" abstract");

            if (typeDefinition.IsAnsiClass)
                builder.Append(" ansi");

            if (typeDefinition.IsSealed)
                builder.Append(" sealed");

            if (typeDefinition.IsBeforeFieldInit)
                builder.Append(" beforefieldinit");

            builder.Append($" {typeDefinition.FullName}");

            if (typeDefinition.BaseType != null)
                builder.Append(
                    $" extends [{typeDefinition.BaseType.GetElementType().Scope.Name}]{typeDefinition.BaseType.FullName}");

            if (typeDefinition.HasInterfaces)
                builder.Append(
                    $" implements {string.Join(", ", typeDefinition.Interfaces.Select(x => x.InterfaceType.FullName))}");

            return builder.ToString();
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
                        builder.Append(",");

                    builder.Append($"{method.Parameters[i].ParameterType.MetadataType.ToString().ToLowerInvariant()} ");
                    builder.Append(method.Parameters[i].Name);
                }
            }
            
            builder.Append(")");
        }
    }
}