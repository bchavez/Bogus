@ECHO OFF

IF NOT DEFINED DevEnvDir (
	IF DEFINED vs120comntools ( 
		CALL "%vs120comntools%\vsvars32.bat"
	)
	IF DEFINED vs140comntools ( 
		CALL "%vs140comntools%\vsvars32.bat"
	)
)

set version="3.0.0.2"
msbuild source\Builder\Builder.csproj
Source\Builder\bin\Debug\fb.exe Source\Builder\bin\Debug\Builder.dll -c:ProjectBuildTask -p:Version="%version%"
