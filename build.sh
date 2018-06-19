#!/bin/bash

dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj $@
dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f netstandard1.6 $@