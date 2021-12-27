@echo on

nuget push *.snupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate
Pause


