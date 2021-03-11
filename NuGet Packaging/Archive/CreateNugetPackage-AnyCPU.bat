@echo off
if exist nuget.exe echo File nuget.exe is present in current folder.
if not exist nuget.exe (
echo File nuget.exe is not present in current folder. Cannot proceed further
PAUSE
EXIT)

Echo Processing NugetPackage

if exist ../_CORE_PROJECTS/HaleyMVVM/HaleyMVVM.csproj nuget pack ../_CORE_PROJECTS/HaleyMVVM/HaleyMVVM.csproj -Prop Configuration=Release -IncludeReferencedProjects  -Symbols -SymbolPackageFormat snupkg
if exist ../_CORE_PROJECTS/HaleyWPF/HaleyWPF.csproj nuget pack ../_CORE_PROJECTS/HaleyWPF/HaleyWPF.csproj -Prop Configuration=Release -IncludeReferencedProjects  -Symbols -SymbolPackageFormat snupkg


Echo Process completed successfully. 
PAUSE


