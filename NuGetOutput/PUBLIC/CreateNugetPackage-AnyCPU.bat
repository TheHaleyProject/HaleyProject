../../FLIPPER__/FLIPPER__/echo off
if exist nuget.exe echo File nuget.exe is present in current folder.
if not exist nuget.exe (
echo File nuget.exe is not present in current folder. Cannot proceed further
PAUSE
EXIT)

if exist ../../FLIPPER__/FlipperFonts/FlipperFonts.csproj nuget pack ../../FLIPPER__/FlipperFonts/FlipperFonts.csproj -Properties Configuration=Release 
if exist ../../FLIPPER__/FlipperUI/FlipperUI.csproj nuget pack ../../FLIPPER__/FlipperUI/FlipperUI.csproj -IncludeReferencedProjects -Prop Configuration=Release  
if exist ../../FLIPPER__/FlipperStyles/FlipperStyles.csproj nuget pack ../../FLIPPER__/FlipperStyles/FlipperStyles.csproj -IncludeReferencedProjects -Prop Configuration=Release  
if exist ../../FLIPPER__/HaleyMVVM/HaleyMVVM.csproj nuget pack ../../FLIPPER__/HaleyMVVM/HaleyMVVM.csproj -Prop Configuration=Release 
if exist ../../FLIPPER__/HLog\HLog.csproj nuget pack ../../FLIPPER__/HLog\HLog.csproj -Prop Configuration=Release 
if exist ../../FLIPPER__/FlipperHelpers\FlipperHelpers.csproj nuget pack ../../FLIPPER__/FlipperHelpers\FlipperHelpers.csproj -Prop Configuration=Release 
if exist ../../FLIPPER__/FlipperHelpers.Standard\Flipper.Helpers.Standard.csproj nuget pack ../../FLIPPER__/FlipperHelpers.Standard\Flipper.Helpers.Standard.csproj -Prop Configuration=Release 
if exist ../../FLIPPER__/HaleyLog/HaleyLog.csproj nuget pack ../../FLIPPER__/HaleyLog/HaleyLog.csproj -Prop Configuration=Release 
Echo Process completed successfully. 
PAUSE


