@echo off
echo ========================================
echo Cargowise Model Generator
echo ========================================
echo.

set "BATCH_DIR=%~dp0"
set "UNIVERSAL_DIR=%BATCH_DIR%\universal"
set "NATIVE_DIR=%BATCH_DIR%\native"
set "LOG_DIR=%BATCH_DIR%Logs"
set "LEGAL_HEADER=%BATCH_DIR%\legal-header.txt"
set "OUTPUT_DIR=%BATCH_DIR%\..\CargoWiseNetLibrary\Models"
set "MAIN_README=%BATCH_DIR%\..\README.md"
set "MODELS_README=%OUTPUT_DIR%\README.md"

echo Output: %OUTPUT_DIR%
echo.

for /f "tokens=2 delims==" %%I in ('wmic os get localdatetime /value') do set datetime=%%I
set "YEAR=%datetime:~0,4%"
set "MONTH=%datetime:~4,2%"
set "DAY=%datetime:~6,2%"

if "%MONTH%"=="01" set "MONTH_NAME=January"
if "%MONTH%"=="02" set "MONTH_NAME=February"
if "%MONTH%"=="03" set "MONTH_NAME=March"
if "%MONTH%"=="04" set "MONTH_NAME=April"
if "%MONTH%"=="05" set "MONTH_NAME=May"
if "%MONTH%"=="06" set "MONTH_NAME=June"
if "%MONTH%"=="07" set "MONTH_NAME=July"
if "%MONTH%"=="08" set "MONTH_NAME=August"
if "%MONTH%"=="09" set "MONTH_NAME=September"
if "%MONTH%"=="10" set "MONTH_NAME=October"
if "%MONTH%"=="11" set "MONTH_NAME=November"
if "%MONTH%"=="12" set "MONTH_NAME=December"

set "DAY=%DAY:~-2%"
if "%DAY:~0,1%"=="0" set "DAY=%DAY:~1%"
set "CURRENT_DATE=%MONTH_NAME% %DAY% %YEAR%"
set "BADGE_DATE=%MONTH_NAME%_%DAY%_%YEAR%"

IF NOT EXIST "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"
IF NOT EXIST "%LOG_DIR%" mkdir "%LOG_DIR%"

echo Cleaning old files...
del /Q "%OUTPUT_DIR%\*.cs" 2>nul
echo.

echo ========================================
echo STEP 1: Generate Models
echo ========================================
echo.

echo Universal schemas...
schema-generator --schema-dir "%UNIVERSAL_DIR%" --output-dirs "%OUTPUT_DIR%" --namespaces "CargoWiseNetLibrary.Models.Universal" --duplicate-log "%LOG_DIR%\universal_duplicates.log" --legal-header "%LEGAL_HEADER%" --remove-duplicates "MessageNumberType"
if %ERRORLEVEL% NEQ 0 ( echo FAILED & pause & exit /b 1 )

echo Native schemas...
schema-generator --schema-dir "%NATIVE_DIR%" --output-dirs "%OUTPUT_DIR%" --namespaces "CargoWiseNetLibrary.Models.Native" --duplicate-log "%LOG_DIR%\native_duplicates.log" --legal-header "%LEGAL_HEADER%" --remove-duplicates "MessageNumberType"
if %ERRORLEVEL% NEQ 0 ( echo FAILED & pause & exit /b 1 )
echo.

echo ========================================
echo STEP 2: Clean File Names
echo ========================================
echo.

powershell -Command "Get-ChildItem -Path '%OUTPUT_DIR%' -Filter *.cs | ForEach-Object { $newName = $_.Name -replace '^.*?\.([^\.]+)\.cs$', '$1.cs'; if ($_.Name -ne $newName) { Rename-Item $_.FullName -NewName $newName -Force; Write-Host ('  ' + $_.Name + ' -> ' + $newName) } }"
echo.

echo ========================================
echo STEP 3: Remove Generated NativeBody
echo ========================================
echo.

powershell -ExecutionPolicy Bypass -File "%BATCH_DIR%\Remove-NativeBody.ps1" -FilePath "%OUTPUT_DIR%\Native.cs"
echo.

echo ========================================
echo STEP 4: Copy Handwritten Files
echo ========================================
echo.

copy /Y "%BATCH_DIR%\handwritten\NativeBody.cs" "%OUTPUT_DIR%\NativeBody.cs"
echo.

echo ========================================
echo STEP 5: Update READMEs
echo ========================================
echo.

powershell -Command "if (Test-Path '%MODELS_README%') { (Get-Content '%MODELS_README%' -Raw) -replace 'Models_Updated-[^)]+', ('Models_Updated-' + '%BADGE_DATE%') -replace '\*\*Last Generated\*\*: [^\n]+', ('**Last Generated**: ' + '%CURRENT_DATE%') | Set-Content '%MODELS_README%' }"
powershell -Command "if (Test-Path '%MAIN_README%') { (Get-Content '%MAIN_README%' -Raw) -replace 'Models_Updated-[^)]+', ('Models_Updated-' + '%BADGE_DATE%') -replace '\*\*Last Updated\*\*: [^\n]+', ('**Last Updated**: ' + '%CURRENT_DATE%') | Set-Content '%MAIN_README%' }"
echo.

echo ========================================
echo Done!
echo ========================================
pause
