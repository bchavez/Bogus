@echo off
cls

.paket\paket.bootstrapper.exe
if errorlevel 1 (
  exit /b %errorlevel%
)

pushd Source
..\.paket\paket.exe install
if errorlevel 1 (
  popd
  exit /b %errorlevel%
)
popd

"Source\packages\build\FAKE\tools\Fake.exe" .\Source\Builder\build.fsx %1
