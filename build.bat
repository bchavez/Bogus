@ECHO OFF
SETLOCAL

set BOGUS_VERSION=3.0.0.3
set TaskRunner=Source\packages\NUnit.Runners.2.6.4\tools\nunit-console.exe Source\Builder\bin\debug\Builder.dll /nodots /nologo /noxml


IF NOT DEFINED DevEnvDir (
	IF DEFINED vs140comntools ( 
		CALL "%vs140comntools%\vsvars32.bat"
	)
)

msbuild source\Builder\Builder.csproj
if %errorlevel% neq 0 exit /b %errorlevel%

ECHO ####################################
ECHO               CLEAN
ECHO ####################################
%TaskRunner% /run:Builder.Tasks.BuildTasks.Clean
if %errorlevel% neq 0 exit /b %errorlevel%

ECHO ####################################
ECHO              Prep
ECHO ####################################
%TaskRunner% /run:Builder.Tasks.BuildTasks.Prep
if %errorlevel% neq 0 exit /b %errorlevel%

ECHO ####################################
ECHO              Build
ECHO ####################################
msbuild @build.MSBuildArgs
if %errorlevel% neq 0 exit /b %errorlevel%

ECHO ####################################
ECHO             Package
ECHO ####################################
%TaskRunner% /run:Builder.Tasks.BuildTasks.Package
if %errorlevel% neq 0 exit /b %errorlevel%