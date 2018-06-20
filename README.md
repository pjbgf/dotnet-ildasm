# Dot Net IL Disassembler  
[![Windows Build](https://ci.appveyor.com/api/projects/status/is3qcb0gnf018vx6/branch/master?svg=true)](https://ci.appveyor.com/project/pjbgf/dotnet-ildasm/branch/master) 
[![CircleCI](https://circleci.com/gh/pjbgf/dotnet-ildasm.svg?style=svg)](https://circleci.com/gh/pjbgf/dotnet-ildasm)
[![Nuget](https://img.shields.io/nuget/dt/dotnet-ildasm.svg)](http://nuget.org/packages/dotnet-ildasm) 
[![Nuget](https://img.shields.io/nuget/v/dotnet-ildasm.svg)](http://nuget.org/packages/dotnet-ildasm) 
[![Known Vulnerabilities](https://snyk.io/test/github/pjbgf/dotnet-ildasm/badge.svg?targetFile=src\dotnet-ildasm\obj\project.assets.json)](https://snyk.io/test/github/pjbgf/dotnet-ildasm)
[![License](http://img.shields.io/:license-mit-blue.svg)](http://pjbgf.mit-license.org)  

# Description
The `dotnet ildasm` provides a command-line IL dissassembler. Simply send the assembly path as a parameter and as a result you will get the IL contents of that assembly.

# Setup
The project was created as a global CLI tool, therefore you can install with a single command:  

`dotnet tool install -g dotnet-ildasm`

Notice that for the command above to work, you need .NET Core SDK 2.1.300 or above installed in your machine.

# Syntax
```
dotnet ildasm <ASSEMBLY_PATH>
dotnet ildasm <ASSEMBLY_PATH> <-o|--output>
dotnet ildasm <ASSEMBLY_PATH> <-i|--item>
dotnet ildasm <-h|--help>
```

# Options
`-i`  
Filter results by method and/or classes to be disassembled.

`-o`  
Define the output file to be created with the assembly's IL.

# Examples
Output IL to the command line:
```
dotnet ildasm myassembly.dll
```

Filter results by method and/or classes to be disassembled, showing the result in the command line:
```
dotnet ildasm myassembly.dll -i ClassName
dotnet ildasm myassembly.dll -i ::MethodName
dotnet ildasm myassembly.dll -i ClassName::MethodName
```

Define the file to be created with the output: 
```
dotnet ildasm myassembly.dll -o disassembledAssembly.il
```
  
# Breaking Changes

* .Net Core 2.1 only support: In order to support the .Net Core CLI Tool model, all the other target frameworks were removed. Please note that you still can download the source code and target it to .Net Core 1, 1.1, 2.0 and net45.
* Default execution now outputs to console, instead of creating an IL file.

# Powered by
This tool was developed and is maintained with JetBrains Rider: the cross-platform and lightweight .NET/C# IDE which comes with ReSharper integrated. For more information check [JetBrains' website](https://www.jetbrains.com/rider).
