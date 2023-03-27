@echo off
cls

SET BUILD_ROOT=.

dotnet tool restore

dotnet nuke %1 --root "%BUILD_ROOT%"