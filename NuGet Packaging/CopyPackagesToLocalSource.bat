@echo off

if exist "\\srv-ireview\InternalFeed\nuget" (if exist *.nupkg copy *.nupkg "\\srv-ireview\InternalFeed\nuget" /y)
if exist "\\srv-ireview\InternalFeed\nuget" (if exist *.snupkg copy *.snupkg "\\srv-ireview\InternalFeed\nuget" /y)

pause


