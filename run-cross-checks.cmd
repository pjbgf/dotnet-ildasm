@ECHO OFF

SETLOCAL EnableDelayedExpansion 

SET ANYCPU_PATH=c:\git\dotnet-ildasm\src\dotnet-ildasm.Sample\bin\Any Cpu\Release
REM SET TARGET_RELEASE_FOLDER=Release
SET TARGET_RELEASE_FOLDER=Any Cpu\Release

ECHO Using target folder: %TARGET_RELEASE_FOLDER%

ECHO Executing dotnet core ildasm against netstandard1.6 library... 
dotnet run --framework netcoreapp1.0 --project src\dotnet-ildasm\dotnet-ildasm.csproj "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\netstandard1.6\dotnet-ildasm.Sample.dll" -o netcore_netstandard16.il

if %ERRORLEVEL% EQU 0 ECHO DONE


ECHO Executing dotnet core ildasm against net45 library...
dotnet run --framework netcoreapp1.0 --project src\dotnet-ildasm\dotnet-ildasm.csproj "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\net45\dotnet-ildasm.Sample.dll" -o netcore_net45.il

if %ERRORLEVEL% EQU 0 ECHO DONE


ECHO Executing net45 ildasm.exe against netstandard1.6 library...
"src\dotnet-ildasm\bin\%TARGET_RELEASE_FOLDER%\net45\dotnet-ildasm.exe" "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\netstandard1.6\dotnet-ildasm.Sample.dll" -o net45_netcore.il

if %ERRORLEVEL% EQU 0 ECHO DONE


ECHO Executing net45 ildasm.exe against net45 library...
"src\dotnet-ildasm\bin\%TARGET_RELEASE_FOLDER%\net45\dotnet-ildasm.exe" "src\dotnet-ildasm.Sample\bin\%TARGET_RELEASE_FOLDER%\net45\dotnet-ildasm.Sample.dll" -o net45_net45.il

if %ERRORLEVEL% EQU 0 ECHO DONE

ENDLOCAL