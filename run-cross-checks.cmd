@ECHO OFF

SETLOCAL EnableDelayedExpansion 

SET ANYCPU_PATH="src\dotnet-ildasm.Sample\bin\Any Cpu\Release"
SET TARGET_RELEASE_FOLDER=Any Cpu\Release

IF NOT EXIST %ANYCPU_PATH% ( 
	SET TARGET_RELEASE_FOLDER=Release
)

ECHO Using target folder: %TARGET_RELEASE_FOLDER%

ECHO Executing dotnet core ildasm against netstandard1.6 library... 
dotnet run --framework netcoreapp2.0 --project src\dotnet-ildasm\dotnet-ildasm.csproj "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\netstandard1.6\dotnet-ildasm.Sample.dll" -o netcore_netstandard16.il

if %ERRORLEVEL% EQU 0 ECHO DONE


ECHO Executing dotnet core ildasm against net45 library...
dotnet run --framework netcoreapp2.0 --project src\dotnet-ildasm\dotnet-ildasm.csproj "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\net45\dotnet-ildasm.Sample.exe" -o netcore_net45.il

if %ERRORLEVEL% EQU 0 ECHO DONE


ECHO Executing net45 ildasm.exe against netstandard1.6 library...
"src\dotnet-ildasm\bin\%TARGET_RELEASE_FOLDER%\net45\dotnet-ildasm.exe" "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\netstandard1.6\dotnet-ildasm.Sample.dll" -o net45_netcore.il

if %ERRORLEVEL% EQU 0 ECHO DONE


ECHO Executing net45 ildasm.exe against net45 library...
"src\dotnet-ildasm\bin\%TARGET_RELEASE_FOLDER%\net45\dotnet-ildasm.exe" "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\net45\dotnet-ildasm.Sample.exe" -o net45_net45.il

if %ERRORLEVEL% EQU 0 ECHO DONE


<<<<<<< HEAD
REM ECHO "Reassembling .IL's onto Portable executable files again."
REM ilasm netcore_netstandard16.il /dll /output:netcore_netstandard16.dll
REM ilasm netcore_net45.il /exe /output:netcore_net45.exe
REM ilasm net45_netcore.il /exe /output:net45_netcore.exe
REM ilasm net45_net45.il /exe /output:net45_net45.exe

=======
>>>>>>> release/0.8.0-mess
ENDLOCAL