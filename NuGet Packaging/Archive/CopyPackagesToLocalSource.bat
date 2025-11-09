@echo off

if exist "C:\_LocalFeed" (if exist *.nupkg copy *.nupkg "C:\_LocalFeed" /y)
if exist "C:\_LocalFeed" (if exist *.snupkg copy *.snupkg "C:\_LocalFeed" /y)

pause


