#!/bin/bash

dotnet build src/dotnet-ildasm/dotnet-ildasm.csproj $@
dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f netstandard1.6 $@

if [[ ! -v RUN_MONO_TESTS ]]
then

    echo "Not building projects for mono."

else

    dotnet build src/dotnet-ildasm.Sample/dotnet-ildasm.Sample.csproj -f net45 $@

fi