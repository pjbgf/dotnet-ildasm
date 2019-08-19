@ECHO OFF

SETLOCAL EnableDelayedExpansion 

SET ANYCPU_PATH="src\dotnet-ildasm.Sample\bin\Any Cpu\Release"
SET TARGET_RELEASE_FOLDER=Any Cpu\Release

IF NOT EXIST %ANYCPU_PATH% ( 
	SET TARGET_RELEASE_FOLDER=Release
)

ECHO Using target folder: %TARGET_RELEASE_FOLDER%

ECHO Executing dotnet core ildasm against netstandard2.0 library... 
dotnet run --project src\dotnet-ildasm\dotnet-ildasm.csproj "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\netstandard2.0\dotnet-ildasm.Sample.dll" -o netcore_netstandard20.il

if %ERRORLEVEL% EQU 0 ECHO DONE


ECHO Executing dotnet core ildasm against net45 library...
dotnet run --project src\dotnet-ildasm\dotnet-ildasm.csproj "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\net45\dotnet-ildasm.Sample.exe" -o netcore_net45.il

if %ERRORLEVEL% EQU 0 ECHO DONE


ENDLOCAL