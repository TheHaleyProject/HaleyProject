@echo off
if exist nuget.exe echo File nuget.exe is present in current folder.
if not exist nuget.exe (
echo File nuget.exe is not present in current folder. Cannot proceed further
PAUSE
EXIT)

Echo Processing NugetPackage
if exist ../_CORE_PROJECTS/HaleyMVVM/HaleyMVVM.csproj nuget pack ../_CORE_PROJECTS/HaleyMVVM/HaleyMVVM.csproj -IncludeReferencedProjects -Prop "Configuration=Release;Platform=x64" 

Echo Processing SymbolsPackage
if exist ../_CORE_PROJECTS/HaleyMVVM/HaleyMVVM.csproj nuget pack ../_CORE_PROJECTS/HaleyMVVM/HaleyMVVM.csproj -IncludeReferencedProjects -Prop "Configuration=Release;Platform=x64"  -Symbols -SymbolPackageFormat snupkg

Echo Process completed successfully. 
PAUSE