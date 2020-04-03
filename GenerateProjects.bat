@echo off
echo Generating Projects

Sharpmake\Sharpmake.Application.exe /sources(@"neuroam\main.sharpmake.cs")

if %ERRORLEVEL% == 0 (
    COLOR 2F
 ) else (
     COLOR 4F
 )

echo Finished generating projects with errorcode = %ERRORLEVEL%