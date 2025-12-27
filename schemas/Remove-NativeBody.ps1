#Requires -Version 5.0

<#
.SYNOPSIS
    Removes NativeBody class from Native.cs (using handwritten version instead).

.PARAMETER FilePath
    Path to the Native.cs file.
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$FilePath
)

if (-not (Test-Path $FilePath)) {
    Write-Host "Error: File not found: $FilePath" -ForegroundColor Red
    exit 1
}

Write-Host "Removing generated NativeBody from Native.cs..." -ForegroundColor Cyan

$content = Get-Content -Path $FilePath -Raw -Encoding UTF8

# Find all NativeBody class occurrences and remove them
$removedCount = 0

while ($true) {
    $classStart = $content.IndexOf('public partial class NativeBody')
    if ($classStart -lt 0) { break }

    $braceStart = $content.IndexOf('{', $classStart)
    if ($braceStart -lt 0) { break }

    # Count braces to find class end
    $depth = 0
    $classEnd = -1
    for ($i = $braceStart; $i -lt $content.Length; $i++) {
        if ($content[$i] -eq '{') { $depth++ }
        elseif ($content[$i] -eq '}') {
            $depth--
            if ($depth -eq 0) {
                $classEnd = $i + 1
                break
            }
        }
    }

    if ($classEnd -lt 0) { break }

    # Find attribute start (go back to find [GeneratedCodeAttribute...)
    $attrStart = $content.LastIndexOf('[GeneratedCodeAttribute', $classStart)
    if ($attrStart -lt 0 -or ($classStart - $attrStart) -gt 500) {
        $attrStart = $classStart
    }

    # Also check for preceding blank lines
    while ($attrStart -gt 0 -and $content[$attrStart - 1] -match '[\r\n\s]') {
        $attrStart--
    }

    # Remove the class
    $content = $content.Substring(0, $attrStart) + $content.Substring($classEnd)
    $removedCount++
    Write-Host "  Removed NativeBody occurrence #$removedCount" -ForegroundColor Green
}

if ($removedCount -eq 0) {
    Write-Host "  No NativeBody class found" -ForegroundColor Yellow
} else {
    Set-Content -Path $FilePath -Value $content -Encoding UTF8 -NoNewline
    Write-Host "Removed $removedCount NativeBody class(es)" -ForegroundColor Green
}

exit 0
