
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Sdk;

namespace DotNet.Ildasm.Tests.Internal
{
    public sealed class IgnoreOnWindowsFact : FactAttribute
    {
        public IgnoreOnWindowsFact()
        {
            if (IsWindows())
            {
                Skip = "Ignore on Windows as underlaying dependencies are not stable";
            }
        }

        private static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    [DataDiscoverer("Xunit.Sdk.InlineDataDiscoverer", "xunit.core")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class IgnoreOnWindowsInlineData : DataAttribute
    {
        readonly object[] data;

        public IgnoreOnWindowsInlineData(params object[] data)
        {
            this.data = data;
            if (IsWindows())
            {
                Skip = "Ignore on Windows as underlaying dependencies are not stable";
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // This is called by the WPA81 version as it does not have access to attribute ctor params
            return new[] { data };
        }

        private static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }
}