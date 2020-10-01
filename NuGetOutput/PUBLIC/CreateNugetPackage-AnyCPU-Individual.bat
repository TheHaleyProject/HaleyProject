@echo off
if exist nuget.exe echo File nuget.exe is present in current folder.
if not exist nuget.exe (
echo File nuget.exe is not present in current folder. Cannot proceed further
PAUSE
EXIT)

if exist ../../_CORE_PROJECTS/HaleyMVVM/HaleyMVVM.csproj nuget pack ../../_CORE_PROJECTS/HaleyMVVM/HaleyMVVM.csproj -Prop Configuration=Release 


Echo Process completed successfully. 
PAUSE


