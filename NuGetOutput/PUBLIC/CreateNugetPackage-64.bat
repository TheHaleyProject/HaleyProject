@echo off
if exist nuget.exe echo File nuget.exe is present in current folder.
if not exist nuget.exe (
echo File nuget.exe is not present in current folder. Cannot proceed further
PAUSE
EXIT)

if exist ../../FLIPPER__/FlipperFonts/FlipperFonts.csproj nuget pack ../../FLIPPER__/FlipperFonts/FlipperFonts.csproj -Properties "Configuration=Release;Platform=x64" 
if exist ../../FLIPPER__/FlipperFonts2/FlipperFonts2.csproj nuget pack ../../FLIPPER__/FlipperFonts2/FlipperFonts2.csproj -Prop "Configuration=Release;Platform=x64" 
if exist ../../FLIPPER__/FlipperUI/FlipperUI.csproj nuget pack ../../FLIPPER__/FlipperUI/FlipperUI.csproj -IncludeReferencedProjects -Prop "Configuration=Release;Platform=x64"  
if exist ../../FLIPPER__/FlipperStyles/FlipperStyles.csproj nuget pack ../../FLIPPER__/FlipperStyles/FlipperStyles.csproj -IncludeReferencedProjects -Prop "Configuration=Release;Platform=x64"  
if exist ../../FLIPPER__/FlipperMVVM/FlipperMVVM.csproj nuget pack ../../FLIPPER__/FlipperMVVM/FlipperMVVM.csproj -IncludeReferencedProjects -Prop "Configuration=Release;Platform=x64" 
if exist ../../FLIPPER__/HaleyLog/HaleyLog.csproj nuget pack ../../FLIPPER__/HaleyLog/HaleyLog.csproj -IncludeReferencedProjects -Prop "Configuration=Release;Platform=x64" 


Echo Process completed successfully. 
PAUSE


