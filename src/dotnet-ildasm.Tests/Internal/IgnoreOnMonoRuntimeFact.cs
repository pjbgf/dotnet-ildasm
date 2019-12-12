
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Sdk;

namespace DotNet.Ildasm.Tests.Internal
{
    public sealed class IgnoreOnMonoFact : FactAttribute
    {
        public IgnoreOnMonoFact()
        {
            if (IsMonoRuntime())
            {
                Skip = "Ignore on Mono as underlaying dependencies are not stable";
            }
        }

        private static bool IsMonoRuntime() => Type.GetType("Mono.Runtime") != null;
    }

    [DataDiscoverer("Xunit.Sdk.InlineDataDiscoverer", "xunit.core")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class IgnoreOnMonoInlineData : DataAttribute
    {
        readonly object[] data;

        public IgnoreOnMonoInlineData(params object[] data)
        {
            this.data = data;
            if (IsMonoRuntime())
            {
                Skip = "Ignore on Mono as underlaying dependencies are not stable";
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // This is called by the WPA81 version as it does not have access to attribute ctor params
            return new[] { data };
        }

        private static bool IsMonoRuntime() => Type.GetType("Mono.Runtime") != null;
    }
}