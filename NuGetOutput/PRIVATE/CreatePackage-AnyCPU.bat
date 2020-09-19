@echo off
if exist nuget.exe echo File nuget.exe is present in current folder.
if not exist nuget.exe (
echo File nuget.exe is not present in current folder. Cannot proceed further
PAUSE
EXIT)

REM if exist ../../CARNIVORA__/Carnivora/CarnivoraEngima.csproj nuget pack ../../CARNIVORA__/Carnivora/CarnivoraEngima.csproj -Properties Configuration=Release 
if exist ../../CARNIVORA__/Carnivora.Engima/Carnivora.Engima.csproj nuget pack ../../CARNIVORA__/Carnivora.Engima/Carnivora.Engima.csproj -Properties Configuration=Release 

Echo Process completed successfully. 
PAUSE