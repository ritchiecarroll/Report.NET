ECHO OFF
SET Path=%Path%;c:\WINDOWS\Microsoft.NET\Framework\v2.0.50727
SET CurPath=%CD%
SET BinPath=%CD%\ReportSamples\Bin

CD %CurPath%\Root\Reports
csc @Reports.csc
IF ERRORLEVEL 1 GOTO End
MOVE Reports.dll %BinPath%

CD %CurPath%\ReportSamples\Samples
csc @Samples.csc
IF ERRORLEVEL 1 GOTO End
MOVE Samples.exe %BinPath%

CD %CurPath%\ReportSamples\StartTemplate
csc @StartTemplate.csc
IF ERRORLEVEL 1 GOTO End
MOVE StartTemplate.exe %BinPath%

CD %CurPath%\ReportSamples\StartTemplateVB
vbc @StartTemplateVB.vbc
IF ERRORLEVEL 1 GOTO End
MOVE StartTemplateVB.exe %BinPath%

CD %CurPath%\ReportSamples\TableLayoutManagerSampleVB
vbc @TableLayoutManagerSampleVB.vbc
IF ERRORLEVEL 1 GOTO End
MOVE TableLayoutManagerSampleVB.exe %BinPath%

ECHO successfully done!
:End
PAUSE
