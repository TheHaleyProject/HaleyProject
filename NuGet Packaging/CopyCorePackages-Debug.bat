@echo off


REM HALEY MVVM
ECHO PROCESSING HALEY MVVM NUPKG
if exist ..\..\HaleyMVVM\HaleyMVVM\bin\Debug\*.nupkg (
xcopy ..\..\HaleyMVVM\HaleyMVVM\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyMVVM\HaleyMVVM\bin\Debug\*.nupkg 
)
ECHO PROCESSING HALEY EXTENSIONS SNUPKG
if exist ..\..\HaleyMVVM\HaleyMVVM\bin\Debug\*.snupkg (
xcopy ..\..\HaleyMVVM\HaleyMVVM\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyMVVM\HaleyMVVM\bin\Debug\*.snupkg 
)

REM HALEY WPF
ECHO PROCESSING HALEY WPF NUPKG
if exist ..\..\HaleyWPF\HaleyWPF\bin\Debug\*.nupkg (
xcopy ..\..\HaleyWPF\HaleyWPF\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyWPF\HaleyWPF\bin\Debug\*.nupkg 
)
ECHO PROCESSING HALEY WPF SNUPKG
if exist ..\..\HaleyWPF\HaleyWPF\bin\Debug\*.snupkg (
xcopy ..\..\HaleyWPF\HaleyWPF\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyWPF\HaleyWPF\bin\Debug\*.snupkg 
)

REM HALEY WPF FONTS
ECHO PROCESSING HALEY WPF FONTS NUPKG
if exist ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Debug\*.nupkg (
xcopy ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Debug\*.nupkg 
)
ECHO PROCESSING HALEY WPF FONTS SNUPKG
if exist ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Debug\*.snupkg (
xcopy ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Debug\*.snupkg 
)

REM HALEY EXTENSIONS
ECHO PROCESSING HALEY EXTENSIONS NUPKG
if exist ..\..\HaleyExtensions\HaleyExtensions\bin\Debug\*.nupkg (
xcopy ..\..\HaleyExtensions\HaleyExtensions\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyExtensions\HaleyExtensions\bin\Debug\*.nupkg 
)
ECHO PROCESSING HALEY EXTENSIONS SNUPKG
if exist ..\..\HaleyExtensions\HaleyExtensions\bin\Debug\*.snupkg (
xcopy ..\..\HaleyExtensions\HaleyExtensions\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyExtensions\HaleyExtensions\bin\Debug\*.snupkg 
)

REM HALEY EVENTS
ECHO PROCESSING HALEY EVENTS NUPKG
if exist ..\..\HaleyEvents\HaleyEvents\bin\Debug\*.nupkg (
xcopy ..\..\HaleyEvents\HaleyEvents\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyEvents\HaleyEvents\bin\Debug\*.nupkg 
)
ECHO PROCESSING HALEY EVENTS SNUPKG
if exist ..\..\HaleyEvents\HaleyEvents\bin\Debug\*.snupkg (
xcopy ..\..\HaleyEvents\HaleyEvents\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyEvents\HaleyEvents\bin\Debug\*.snupkg 
)

REM HALEY IOC
ECHO PROCESSING HALEY IOC NUPKG
if exist ..\..\HaleyIOC\HaleyIOC\bin\Debug\*.nupkg (
xcopy ..\..\HaleyIOC\HaleyIOC\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyIOC\HaleyIOC\bin\Debug\*.nupkg 
)
ECHO PROCESSING HALEY IOC SNUPKG
if exist ..\..\HaleyIOC\HaleyIOC\bin\Debug\*.snupkg (
xcopy ..\..\HaleyIOC\HaleyIOC\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyIOC\HaleyIOC\bin\Debug\*.snupkg 
)

REM HALEY ABSTRACTIONS
ECHO PROCESSING HALEY ABSTRACTIONS NUPKG
if exist ..\..\HaleyAbstractions\HaleyAbstractions\bin\Debug\*.nupkg (
xcopy ..\..\HaleyAbstractions\HaleyAbstractions\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyAbstractions\HaleyAbstractions\bin\Debug\*.nupkg 
)
ECHO PROCESSING HALEY ABSTRACTIONS SNUPKG
if exist ..\..\HaleyAbstractions\HaleyAbstractions\bin\Debug\*.snupkg (
xcopy ..\..\HaleyAbstractions\HaleyAbstractions\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyAbstractions\HaleyAbstractions\bin\Debug\*.snupkg 
)

REM HALEY HELPERS
ECHO PROCESSING HALEY HELPERS NUPKG
if exist ..\..\HaleyHelpers\HaleyHelpers\bin\Debug\*.nupkg (
xcopy ..\..\HaleyHelpers\HaleyHelpers\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyHelpers\HaleyHelpers\bin\Debug\*.nupkg 
)
ECHO PROCESSING HALEY HELPERS SNUPKG
if exist ..\..\HaleyHelpers\HaleyHelpers\bin\Debug\*.snupkg (
xcopy ..\..\HaleyHelpers\HaleyHelpers\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyHelpers\HaleyHelpers\bin\Debug\*.snupkg 
)

REM HALEY LOG
ECHO PROCESSING HALEY LOG NUPKG
if exist ..\..\HaleyLog\HaleyLog\bin\Debug\*.nupkg (
xcopy ..\..\HaleyLog\HaleyLog\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyLog\HaleyLog\bin\Debug\*.nupkg 
)

ECHO PROCESSING HALEY LOG SNUPKG
if exist ..\..\HaleyLog\HaleyLog\bin\Debug\*.snupkg (
xcopy ..\..\HaleyLog\HaleyLog\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyLog\HaleyLog\bin\Debug\*.snupkg 
)

REM HALEY REST
ECHO PROCESSING HALEY REST NUPKG
if exist ..\..\HaleyRest\HaleyRest\bin\Debug\*.nupkg (
xcopy ..\..\HaleyRest\HaleyRest\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyRest\HaleyRest\bin\Debug\*.nupkg 
)

ECHO PROCESSING HALEY REST SNUPKG
if exist ..\..\HaleyRest\HaleyRest\bin\Debug\*.snupkg (
xcopy ..\..\HaleyRest\HaleyRest\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyRest\HaleyRest\bin\Debug\*.snupkg 
)

REM HALEY RULE ENGINE
ECHO PROCESSING HALEY RULE ENGINE NUPKG
if exist ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Debug\*.nupkg (
xcopy ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Debug\*.nupkg .\ /i /y
del /f ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Debug\*.nupkg 
)
ECHO PROCESSING HALEY RULE ENGINE SNUPKG
if exist ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Debug\*.snupkg (
xcopy ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Debug\*.snupkg .\ /i /y
del /f ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Debug\*.snupkg 
)

PAUSE
