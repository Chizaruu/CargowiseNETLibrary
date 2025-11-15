@echo off
echo ========================================
echo Cargowise Model Generator with Class Merging
echo ========================================
echo.

REM Set paths relative to the batch file location
set "BATCH_DIR=%~dp0"
set "UNIVERSAL_DIR=%BATCH_DIR%\universal"
set "NATIVE_DIR=%BATCH_DIR%\native"
set "LOG_DIR=%BATCH_DIR%Logs"
set "LEGAL_HEADER=%BATCH_DIR%\legal-header.txt"

echo Universal schemas: %UNIVERSAL_DIR%
echo Native schemas: %NATIVE_DIR%
echo Log directory: %LOG_DIR%
echo Legal header: %LEGAL_HEADER%
echo.

REM Define output directories and README paths
set "OUTPUT_DIR=%BATCH_DIR%\..\CargoWiseNetLibrary\Models"
set "MAIN_README=%BATCH_DIR%\..\..\README.md"
set "MODELS_README=%OUTPUT_DIR%\README.md"

echo Models will be generated to: %OUTPUT_DIR%
echo.

REM Get current date in readable format
for /f "tokens=2 delims==" %%I in ('wmic os get localdatetime /value') do set datetime=%%I
set "YEAR=%datetime:~0,4%"
set "MONTH=%datetime:~4,2%"
set "DAY=%datetime:~6,2%"

REM Convert month number to name
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

REM Remove leading zero from day
set "DAY=%DAY:~-2%"
if "%DAY:~0,1%"=="0" set "DAY=%DAY:~1%"

set "CURRENT_DATE=%MONTH_NAME% %DAY% %YEAR%"
set "BADGE_DATE=%MONTH_NAME%_%DAY%_%YEAR%"

echo Generation Date: %CURRENT_DATE%
echo Badge Date Format: %BADGE_DATE%
echo.

REM Create output directories
IF NOT EXIST "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"
IF NOT EXIST "%LOG_DIR%" mkdir "%LOG_DIR%"

REM Clean old generated files before regenerating
echo Cleaning old generated files...
del /Q "%OUTPUT_DIR%\*.cs" 2>nul
echo.

REM ========================================
REM STEP 1: Generate models from XSD schemas
REM ========================================

echo ========================================
echo STEP 1: Generating Models from Schemas
echo ========================================
echo.

echo Processing Universal schemas...
schema-generator --schema-dir "%UNIVERSAL_DIR%" --output-dirs "%OUTPUT_DIR%" --namespaces "CargoWiseNetLibrary.Models.Universal" --duplicate-log "%LOG_DIR%\universal_duplicates.log" --legal-header "%LEGAL_HEADER%" --remove-duplicates "MessageNumberType"
if %ERRORLEVEL% NEQ 0 (
    echo Error: Universal schema generation failed!
    pause
    exit /b 1
)
echo.

echo Processing Native schemas...
schema-generator --schema-dir "%NATIVE_DIR%" --output-dirs "%OUTPUT_DIR%" --namespaces "CargoWiseNetLibrary.Models.Native" --duplicate-log "%LOG_DIR%\native_duplicates.log" --legal-header "%LEGAL_HEADER%" --remove-duplicates "MessageNumberType"
if %ERRORLEVEL% NEQ 0 (
    echo Error: Native schema generation failed!
    pause
    exit /b 1
)
echo.

REM ========================================
REM STEP 2: Clean file names
REM ========================================

echo ========================================
echo STEP 2: Cleaning File Names
echo ========================================
echo.

echo Cleaning model file names...
powershell -Command "Get-ChildItem -Path '%OUTPUT_DIR%' -Filter *.cs -Recurse | ForEach-Object { $newName = $_.Name -replace '^.*?\.([^\.]+)\.cs$', '$1.cs'; if ($_.Name -ne $newName) { Rename-Item -Path $_.FullName -NewName $newName -Force; Write-Host ('Renamed: ' + $_.Name + ' -> ' + $newName) } }"
echo.



REM ========================================
REM STEP 3: Merge duplicate class definitions in Native.cs (Native/NativeBody)
REM ========================================

echo ========================================
echo STEP 3: Merging Duplicate Classes (Native.cs)
echo ========================================
echo.

echo Running class-merger on Native.cs...
if exist "%OUTPUT_DIR%\Native.cs" (
    class-merger "%OUTPUT_DIR%\Native.cs"
    if %ERRORLEVEL% NEQ 0 (
        echo Warning: Class merger encountered issues
        echo Check output for details
    )
) else (
    echo Warning: Native.cs not found, skipping merge step
    echo Generated files:
    dir /b "%OUTPUT_DIR%\*.cs"
)
echo.

REM ========================================
REM STEP 4: Update READMEs
REM ========================================

echo ========================================
echo STEP 4: Updating Documentation
echo ========================================
echo.

REM Update Models README with generation date and badge
echo Updating Models README...
powershell -Command "if (Test-Path '%MODELS_README%') { $content = Get-Content '%MODELS_README%' -Raw; $content = $content -replace 'Models_Updated-[^)]+', ('Models_Updated-' + '%BADGE_DATE%'); $content = $content -replace '\*\*Last Generated\*\*: [^\n]+', ('**Last Generated**: ' + '%CURRENT_DATE%'); Set-Content '%MODELS_README%' -Value $content } else { Write-Host 'Models README not found, skipping...' }"
echo.

REM Update Main README with generation date and badge
echo Updating Main README...
powershell -Command "if (Test-Path '%MAIN_README%') { $content = Get-Content '%MAIN_README%' -Raw; $content = $content -replace 'Models_Updated-[^)]+', ('Models_Updated-' + '%BADGE_DATE%'); $content = $content -replace '\*\*Last Updated\*\*: [^\n]+', ('**Last Updated**: ' + '%CURRENT_DATE%'); Set-Content '%MAIN_README%' -Value $content } else { Write-Host 'Main README not found, skipping...' }"
echo.

echo ========================================
echo Model generation complete!
echo ========================================
echo.
echo Generated models: %OUTPUT_DIR%
echo Duplicate logs: %LOG_DIR%
echo Updated README files with:
echo   - Date: %CURRENT_DATE%
echo   - Badge: Models_Updated-%BADGE_DATE%
echo.
echo ========================================
echo Process Summary:
echo ========================================
echo 1. Generated C# models from XSD schemas
echo    - Universal: Consolidated with MessageNumberType pre-removed
echo    - Native: Consolidated with MessageNumberType pre-removed
echo 2. Merged duplicate classes in Native.cs (Native/NativeBody)
echo 3. Cleaned up file names
echo 4. Updated documentation
echo.
echo IMPORTANT: Review the merged classes to ensure correctness
echo Check %LOG_DIR%\universal_duplicates.log and native_duplicates.log
echo ========================================
pause
