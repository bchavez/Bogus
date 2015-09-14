@ECHO OFF
SETLOCAL

set BUILD_VERSION=3.0.1.4

IF NOT DEFINED DevEnvDir (
	IF DEFINED vs140comntools ( 
		CALL "%vs140comntools%\vsvars32.bat"
	)
)

nuget restore Source\Bogus.sln

msbuild Source\Builder\Builder.csproj
if %errorlevel% neq 0 exit /b %errorlevel%

ECHO ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
ECHO        RUNNING BAU BUILDER
ECHO ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Source\Builder\bin\Debug\Builder.exe %1
if %errorlevel% neq 0 exit /b %errorlevel%
