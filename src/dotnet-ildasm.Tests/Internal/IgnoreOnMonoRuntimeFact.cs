
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace DotNet.Ildasm.Tests.Internal
{    public sealed class IgnoreOnMonoRuntimeFact : FactAttribute
    {
        public IgnoreOnMonoRuntimeFact() {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && IsMonoRuntime()) {
                Skip = "Ignore on Linux when run via AppVeyor";
            }
        }

        private static bool IsMonoRuntime() => Type.GetType("Mono.Runtime") != null;
    }
}