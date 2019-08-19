#!/bin/bash

echo "[dotnet core] Executing ildasm against netstandard1.6 library..."
rm netcore_netstandard16.il
dotnet run --project src/dotnet-ildasm/dotnet-ildasm.csproj src/dotnet-ildasm.Sample/bin/Release/netstandard1.6/dotnet-ildasm.Sample.dll -o netcore_netstandard16.il

if [[ ! -v RUN_REASSEMBLE_TESTS ]]
then

    echo "Not running reassembling tests."

else

	echo "Reassembling .IL's onto Portable executable files again."
	ilasm netcore_netstandard16.il /dll /output:netcore_netstandard16.dll

fi