@echo on

nuget.exe push -Source https://rmsofficial.pkgs.visualstudio.com/_packaging/Haley/nuget/v3/index.json -ApiKey AzureDevOps *.nupkg
Pause