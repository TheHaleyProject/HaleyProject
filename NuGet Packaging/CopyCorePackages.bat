@echo off


REM HALEY MVVM
ECHO PROCESSING HALEY MVVM NUPKG
if exist ..\..\HaleyMVVM\HaleyMVVM\bin\Release\*.nupkg (
xcopy ..\..\HaleyMVVM\HaleyMVVM\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyMVVM\HaleyMVVM\bin\Release\*.nupkg 
)
ECHO PROCESSING HALEY EXTENSIONS SNUPKG
if exist ..\..\HaleyMVVM\HaleyMVVM\bin\Release\*.snupkg (
xcopy ..\..\HaleyMVVM\HaleyMVVM\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyMVVM\HaleyMVVM\bin\Release\*.snupkg 
)

REM HALEY WPF
ECHO PROCESSING HALEY WPF NUPKG
if exist ..\..\HaleyWPF\HaleyWPF\bin\Release\*.nupkg (
xcopy ..\..\HaleyWPF\HaleyWPF\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyWPF\HaleyWPF\bin\Release\*.nupkg 
)
ECHO PROCESSING HALEY WPF SNUPKG
if exist ..\..\HaleyWPF\HaleyWPF\bin\Release\*.snupkg (
xcopy ..\..\HaleyWPF\HaleyWPF\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyWPF\HaleyWPF\bin\Release\*.snupkg 
)

REM HALEY WPF FONTS
ECHO PROCESSING HALEY WPF FONTS NUPKG
if exist ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Release\*.nupkg (
xcopy ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Release\*.nupkg 
)
ECHO PROCESSING HALEY WPF FONTS SNUPKG
if exist ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Release\*.snupkg (
xcopy ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyWPFFonts\HaleyWPFFonts\bin\Release\*.snupkg 
)

REM HALEY EXTENSIONS
ECHO PROCESSING HALEY EXTENSIONS NUPKG
if exist ..\..\HaleyExtensions\HaleyExtensions\bin\Release\*.nupkg (
xcopy ..\..\HaleyExtensions\HaleyExtensions\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyExtensions\HaleyExtensions\bin\Release\*.nupkg 
)
ECHO PROCESSING HALEY EXTENSIONS SNUPKG
if exist ..\..\HaleyExtensions\HaleyExtensions\bin\Release\*.snupkg (
xcopy ..\..\HaleyExtensions\HaleyExtensions\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyExtensions\HaleyExtensions\bin\Release\*.snupkg 
)

REM HALEY EVENTS
ECHO PROCESSING HALEY EVENTS NUPKG
if exist ..\..\HaleyEvents\HaleyEvents\bin\Release\*.nupkg (
xcopy ..\..\HaleyEvents\HaleyEvents\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyEvents\HaleyEvents\bin\Release\*.nupkg 
)
ECHO PROCESSING HALEY EVENTS SNUPKG
if exist ..\..\HaleyEvents\HaleyEvents\bin\Release\*.snupkg (
xcopy ..\..\HaleyEvents\HaleyEvents\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyEvents\HaleyEvents\bin\Release\*.snupkg 
)

REM HALEY IOC
ECHO PROCESSING HALEY IOC NUPKG
if exist ..\..\HaleyIOC\HaleyIOC\bin\Release\*.nupkg (
xcopy ..\..\HaleyIOC\HaleyIOC\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyIOC\HaleyIOC\bin\Release\*.nupkg 
)
ECHO PROCESSING HALEY IOC SNUPKG
if exist ..\..\HaleyIOC\HaleyIOC\bin\Release\*.snupkg (
xcopy ..\..\HaleyIOC\HaleyIOC\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyIOC\HaleyIOC\bin\Release\*.snupkg 
)

REM HALEY ABSTRACTIONS
ECHO PROCESSING HALEY ABSTRACTIONS NUPKG
if exist ..\..\HaleyAbstractions\HaleyAbstractions\bin\Release\*.nupkg (
xcopy ..\..\HaleyAbstractions\HaleyAbstractions\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyAbstractions\HaleyAbstractions\bin\Release\*.nupkg 
)
ECHO PROCESSING HALEY ABSTRACTIONS SNUPKG
if exist ..\..\HaleyAbstractions\HaleyAbstractions\bin\Release\*.snupkg (
xcopy ..\..\HaleyAbstractions\HaleyAbstractions\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyAbstractions\HaleyAbstractions\bin\Release\*.snupkg 
)

REM HALEY HELPERS
ECHO PROCESSING HALEY HELPERS NUPKG
if exist ..\..\HaleyHelpers\HaleyHelpers\bin\Release\*.nupkg (
xcopy ..\..\HaleyHelpers\HaleyHelpers\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyHelpers\HaleyHelpers\bin\Release\*.nupkg 
)
ECHO PROCESSING HALEY HELPERS SNUPKG
if exist ..\..\HaleyHelpers\HaleyHelpers\bin\Release\*.snupkg (
xcopy ..\..\HaleyHelpers\HaleyHelpers\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyHelpers\HaleyHelpers\bin\Release\*.snupkg 
)

REM HALEY LOG
ECHO PROCESSING HALEY LOG NUPKG
if exist ..\..\HaleyLog\HaleyLog\bin\Release\*.nupkg (
xcopy ..\..\HaleyLog\HaleyLog\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyLog\HaleyLog\bin\Release\*.nupkg 
)

ECHO PROCESSING HALEY LOG SNUPKG
if exist ..\..\HaleyLog\HaleyLog\bin\Release\*.snupkg (
xcopy ..\..\HaleyLog\HaleyLog\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyLog\HaleyLog\bin\Release\*.snupkg 
)

REM HALEY REST
ECHO PROCESSING HALEY REST NUPKG
if exist ..\..\HaleyRest\HaleyRest\bin\Release\*.nupkg (
xcopy ..\..\HaleyRest\HaleyRest\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyRest\HaleyRest\bin\Release\*.nupkg 
)

ECHO PROCESSING HALEY REST SNUPKG
if exist ..\..\HaleyRest\HaleyRest\bin\Release\*.snupkg (
xcopy ..\..\HaleyRest\HaleyRest\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyRest\HaleyRest\bin\Release\*.snupkg 
)

REM HALEY RULE ENGINE
ECHO PROCESSING HALEY RULE ENGINE NUPKG
if exist ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Release\*.nupkg (
xcopy ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Release\*.nupkg .\ /i /y
del /f ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Release\*.nupkg 
)
ECHO PROCESSING HALEY RULE ENGINE SNUPKG
if exist ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Release\*.snupkg (
xcopy ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Release\*.snupkg .\ /i /y
del /f ..\..\HaleyRuleEngine\HaleyRuleEngine\bin\Release\*.snupkg 
)

PAUSE
