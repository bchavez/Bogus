set version="3.0.0.1"
msbuild source\Builder\Builder.csproj
Source\Builder\bin\Debug\fb.exe Source\Builder\bin\Debug\Builder.dll -c:ProjectBuildTask -p:Version="%version%"
