@echo on

nuget push *.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate
Pause


