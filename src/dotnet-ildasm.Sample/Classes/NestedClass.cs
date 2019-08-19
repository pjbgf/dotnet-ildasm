using System;

namespace dotnet_ildasm.Sample.Classes
{
    public class ParentClass 
    {
		public class NestedClass {
			public string SomeMethod() { return "aaaaa"; }
		}

		public NestedClass MakeFooBar() {
			return new NestedClass();
		}
	}
}
