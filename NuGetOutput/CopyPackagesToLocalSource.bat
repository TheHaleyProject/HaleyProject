@echo off

if exist D:\00-LocalPackages (if exist *.nupkg copy *.nupkg D:\00-LocalPackages /y)
if exist D:\00-LocalPackages (if exist *.snupkg copy *.snupkg D:\00-LocalPackages /y)


