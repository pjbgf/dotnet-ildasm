#!/bin/bash

dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netcoreapp2.2 $@
dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj -f netcoreapp2.1 $@
dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f netstandard2.0 $@