#!/bin/bash

echo "[dotnet core] Executing ildasm against netstandard2.0 library..."
rm -f netcore_netstandard20.il
dotnet run --project src/dotnet-ildasm/dotnet-ildasm.csproj src/dotnet-ildasm.Sample/bin/Release/netstandard2.0/dotnet-ildasm.Sample.dll -o netcore_netstandard20.il

if [[ ! -v RUN_REASSEMBLE_TESTS ]]
then

    echo "Not running reassembling tests."

else

	echo "Reassembling .IL's onto Portable executable files again."
	ilasm netcore_netstandard20.il /dll /output:netcore_netstandard20.dll

fi