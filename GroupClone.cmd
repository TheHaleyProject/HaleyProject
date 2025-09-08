@echo off
echo Move to previous folder
cd..
cd

echo Cloning folders from Haley Project
rem echo Haley | xargs -n1 | xargs -I{} git clone https://github.com/TheHaleyProject/Haley*

for %%G in (HaleyHelpers.DB HaleyHelpers.Web HaleyWorkFlow HaleyWorkFlow.API HaleyHelpers HaleyRest HaleyExtensions HaleyLog HaleyWPF HaleyMVVM HaleyIOC HaleyWPFIconsPack HaleyVue HaleyAbstractions HaleyRuleEngine HaleyEvents HaleyXamlShared HaleyVSIX HaleyWPFFonts) do ((echo %%G) && (git clone https://github.com/TheHaleyProject/%%G))
pause