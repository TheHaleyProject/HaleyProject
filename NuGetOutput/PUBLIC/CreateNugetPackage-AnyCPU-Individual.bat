@echo off
if exist nuget.exe echo File nuget.exe is present in current folder.
if not exist nuget.exe (
echo File nuget.exe is not present in current folder. Cannot proceed further
PAUSE
EXIT)

if exist ../../FLIPPER__/HaleyLog/HaleyLog.csproj nuget pack ../../FLIPPER__/HaleyLog/HaleyLog.csproj -Prop Configuration=Release 


Echo Process completed successfully. 
PAUSE


