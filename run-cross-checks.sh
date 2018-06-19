#!/bin/bash

echo "[dotnet core 2.1] Executing ildasm against netstandard1.6 library..."
dotnet run --project src/dotnet-ildasm/dotnet-ildasm.csproj src/dotnet-ildasm.Sample/bin/Release/netstandard1.6/dotnet-ildasm.Sample.dll -o netcore_netstandard16.il

if [[ ! -v RUN_REASSEMBLE_TESTS ]]
then

    echo "Not running reassembling tests."

else

	echo "Reassembling .IL's onto Portable executable files again."
	ilasm netcore_netstandard16.il /dll /output:netcore_netstandard16.dll
	ilasm netcore_net45.il /exe /output:netcore_net45.exe
	ilasm net45_netcore.il /exe /output:net45_netcore.exe
	ilasm net45_net45.il /exe /output:net45_net45.exe

fi