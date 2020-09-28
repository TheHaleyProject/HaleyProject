../../FLIPPER__/FLIPPER__/echo off
if exist nuget.exe echo File nuget.exe is present in current folder.
if not exist nuget.exe (
echo File nuget.exe is not present in current folder. Cannot proceed further
PAUSE
EXIT)


if exist ../../FLIPPER__/FlipperUI/FlipperUI.csproj nuget pack ../../FLIPPER__/FlipperUI/FlipperUI.csproj -IncludeReferencedProjects -Prop Configuration=Release  

if exist ../../FLIPPER__/HaleyMVVM/HaleyMVVM.csproj nuget pack ../../FLIPPER__/HaleyMVVM/HaleyMVVM.csproj -Prop Configuration=Release 

if exist ../../FLIPPER__/HLog\HLog.csproj nuget pack ../../FLIPPER__/HLog\HLog.csproj -Prop Configuration=Release 

if exist ../../FLIPPER__/HaleyHelpers\HaleyHelpers.csproj nuget pack ../../FLIPPER__/HaleyHelpers\HaleyHelpers.csproj -Prop Configuration=Release 

if exist ../../FLIPPER__/HaleyLog/HaleyLog.csproj nuget pack ../../FLIPPER__/HaleyLog/HaleyLog.csproj -Prop Configuration=Release 
Echo Process completed successfully. 
PAUSE


