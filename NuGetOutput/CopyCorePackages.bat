@echo off

if exist ..\_CORE_PROJECTS\HaleyMVVM\bin\Release\*.nupkg (
xcopy ..\_CORE_PROJECTS\HaleyMVVM\bin\Release\*.nupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyMVVM\bin\Release\*.nupkg 
)

if exist ..\_CORE_PROJECTS\HaleyMVVM\bin\Release\*.snupkg (
xcopy ..\_CORE_PROJECTS\HaleyMVVM\bin\Release\*.snupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyMVVM\bin\Release\*.snupkg 
)

if exist ..\_CORE_PROJECTS\HaleyWPF\bin\Release\*.nupkg (
xcopy ..\_CORE_PROJECTS\HaleyWPF\bin\Release\*.nupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyWPF\bin\Release\*.nupkg 
)

if exist ..\_CORE_PROJECTS\HaleyWPF\bin\Release\*.snupkg (
xcopy ..\_CORE_PROJECTS\HaleyWPF\bin\Release\*.snupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyWPF\bin\Release\*.snupkg 
)

if exist ..\_CORE_PROJECTS\HaleyExtensions\bin\Release\*.nupkg (
xcopy ..\_CORE_PROJECTS\HaleyExtensions\bin\Release\*.nupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyExtensions\bin\Release\*.nupkg 
)

if exist ..\_CORE_PROJECTS\HaleyExtensions\bin\Release\*.snupkg (
xcopy ..\_CORE_PROJECTS\HaleyExtensions\bin\Release\*.snupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyExtensions\bin\Release\*.snupkg 
)

if exist ..\_CORE_PROJECTS\HaleyEvents\bin\Release\*.nupkg (
xcopy ..\_CORE_PROJECTS\HaleyEvents\bin\Release\*.nupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyEvents\bin\Release\*.nupkg 
)

if exist ..\_CORE_PROJECTS\HaleyEvents\bin\Release\*.snupkg (
xcopy ..\_CORE_PROJECTS\HaleyEvents\bin\Release\*.snupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyEvents\bin\Release\*.snupkg 
)

if exist ..\_CORE_PROJECTS\HaleyIOC\bin\Release\*.nupkg (
xcopy ..\_CORE_PROJECTS\HaleyIOC\bin\Release\*.nupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyIOC\bin\Release\*.nupkg 
)

if exist ..\_CORE_PROJECTS\HaleyIOC\bin\Release\*.snupkg (
xcopy ..\_CORE_PROJECTS\HaleyIOC\bin\Release\*.snupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyIOC\bin\Release\*.snupkg 
)


if exist ..\_CORE_PROJECTS\HaleyHelpers\bin\Release\*.nupkg (
xcopy ..\_CORE_PROJECTS\HaleyHelpers\bin\Release\*.nupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyHelpers\bin\Release\*.nupkg 
)

if exist ..\_CORE_PROJECTS\HaleyHelpers\bin\Release\*.snupkg (
xcopy ..\_CORE_PROJECTS\HaleyHelpers\bin\Release\*.snupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyHelpers\bin\Release\*.snupkg 
)

if exist ..\_CORE_PROJECTS\HaleyLog\bin\Release\*.nupkg (
xcopy ..\_CORE_PROJECTS\HaleyLog\bin\Release\*.nupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyLog\bin\Release\*.nupkg 
)

if exist ..\_CORE_PROJECTS\HaleyLog\bin\Release\*.snupkg (
xcopy ..\_CORE_PROJECTS\HaleyLog\bin\Release\*.snupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyLog\bin\Release\*.snupkg 
)

if exist ..\_CORE_PROJECTS\HaleyRuleEngine\bin\Release\*.nupkg (
xcopy ..\_CORE_PROJECTS\HaleyRuleEngine\bin\Release\*.nupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyRuleEngine\bin\Release\*.nupkg 
)

if exist ..\_CORE_PROJECTS\HaleyRuleEngine\bin\Release\*.snupkg (
xcopy ..\_CORE_PROJECTS\HaleyRuleEngine\bin\Release\*.snupkg .\ /i /y
del /f ..\_CORE_PROJECTS\HaleyRuleEngine\bin\Release\*.snupkg 
)

PAUSE
