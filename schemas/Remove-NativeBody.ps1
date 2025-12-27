#Requires -Version 5.0

<#
.SYNOPSIS
    Normalizes namespaces and removes NativeBody class from Native.cs.

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

Write-Host "Processing Native.cs..." -ForegroundColor Cyan

# Read file content
$content = [System.IO.File]::ReadAllText($FilePath)

# Step 1: Normalize namespaces
Write-Host "  Normalizing namespaces..." -ForegroundColor Gray
$content = $content.Replace(
    'Namespace="http://www.cargowise.com/Schemas/Native"]',
    'Namespace="http://www.cargowise.com/Schemas/Native/2011/11"]'
)

# Step 2: Remove all NativeBody classes
Write-Host "  Removing NativeBody classes..." -ForegroundColor Gray
$removedCount = 0

while ($true) {
    $classStart = $content.IndexOf('public partial class NativeBody')
    if ($classStart -lt 0) { break }

    $braceStart = $content.IndexOf('{', $classStart)
    if ($braceStart -lt 0) { break }

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

    $attrStart = $content.LastIndexOf('[GeneratedCodeAttribute', $classStart)
    if ($attrStart -lt 0 -or ($classStart - $attrStart) -gt 500) {
        $attrStart = $classStart
    }

    while ($attrStart -gt 0 -and $content[$attrStart - 1] -match '[\r\n\s]') {
        $attrStart--
    }

    $content = $content.Substring(0, $attrStart) + $content.Substring($classEnd)
    $removedCount++
}

Write-Host "  Removed $removedCount NativeBody class(es)" -ForegroundColor Green

# Write file
[System.IO.File]::WriteAllText($FilePath, $content)
Write-Host "Done" -ForegroundColor Green

exit 0
