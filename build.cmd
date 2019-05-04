@echo off
cls

SET BUILDER=Source\Builder

IF NOT EXIST "%BUILDER%\fake.exe" (
  dotnet tool install fake-cli ^
    --tool-path ./%BUILDER% ^
    --version 5.13.5
)

if errorlevel 1 (
  exit /b %errorlevel%
)

"%BUILDER%/fake.exe" run %BUILDER%\build.fsx target %1

if errorlevel 1 (
  exit /b %errorlevel%
)
