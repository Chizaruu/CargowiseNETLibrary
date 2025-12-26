#Requires -Version 5.0

<#
.SYNOPSIS
    Fixes NativeBody collections to use correct wrapper types instead of CriteriaGroupType.

.DESCRIPTION
    Post-processes generated Native.cs file to replace Collection<CriteriaGroupType> with
    the correct wrapper types (ProductData, OrganizationData, etc.) while preserving the
    Any collection as a fallback for new CargoWise entity types.

    Can auto-detect the file if given a directory path.

.PARAMETER FilePath
    Path to the Native.cs file to fix, or path to directory containing it.

.EXAMPLE
    .\Fix-NativeBody.ps1 -FilePath ".\Models\Native.cs"

.EXAMPLE
    .\Fix-NativeBody.ps1 -FilePath ".\Models"
    # Auto-detects Native.cs or CargoWiseNetLibrary.Models.Native.cs

.EXAMPLE
    .\Fix-NativeBody.ps1 "C:\Project\Models\Native.cs"
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$FilePath
)

$TypeMappings = @(
    @{ Collection = 'Product'; Type = 'ProductData' }
    @{ Collection = 'Organization'; Type = 'OrganizationData' }
    @{ Collection = 'Company'; Type = 'CompanyData' }
    @{ Collection = 'Container'; Type = 'ContainerData' }
    @{ Collection = 'Country'; Type = 'CountryData' }
    @{ Collection = 'Airline'; Type = 'AirlineData' }
    @{ Collection = 'Vessel'; Type = 'VesselData' }
    @{ Collection = 'Rate'; Type = 'RateData' }
    @{ Collection = 'Staff'; Type = 'StaffData' }
    @{ Collection = 'Tag'; Type = 'TagData' }
    @{ Collection = 'TagRule'; Type = 'TagRuleData' }
    @{ Collection = 'CommodityCode'; Type = 'CommodityCodeData' }
    @{ Collection = 'DangerousGood'; Type = 'DangerousGoodData' }
    @{ Collection = 'ServiceLevel'; Type = 'ServiceLevelData' }
    @{ Collection = 'UNLOCO'; Type = 'UNLOCOData' }
    @{ Collection = 'CurrencyExchangeRate'; Type = 'CurrencyExchangeRateData' }
    @{ Collection = 'CusStatement'; Type = 'CusStatementData' }
    @{ Collection = 'BMSystem'; Type = 'BMSystemData' }
    @{ Collection = 'AcceptabilityBand'; Type = 'AcceptabilityBandData' }
    @{ Collection = 'WorkflowTemplate'; Type = 'WorkflowTemplateData' }
)

$Namespace = 'http://www.cargowise.com/Schemas/Native/2011/11'

function Convert-ToCamelCase {
    param([string]$PascalCase)

    if ([string]::IsNullOrEmpty($PascalCase)) {
        return $PascalCase
    }

    return $PascalCase.Substring(0, 1).ToLowerInvariant() + $PascalCase.Substring(1)
}

# Auto-detect file if directory is provided
if (Test-Path $FilePath -PathType Container) {
    Write-Host "Directory provided, searching for Native file..." -ForegroundColor Cyan

    # Try to find Native.cs or CargoWiseNetLibrary.Models.Native.cs
    $possibleFiles = @(
        Join-Path $FilePath "Native.cs"
        Join-Path $FilePath "CargoWiseNetLibrary.Models.Native.cs"
    )

    $foundFile = $possibleFiles | Where-Object { Test-Path $_ } | Select-Object -First 1

    if ($foundFile) {
        Write-Host "  Found: $(Split-Path $foundFile -Leaf)" -ForegroundColor Green
        $FilePath = $foundFile
    } else {
        Write-Host "  Error: Could not find Native.cs or CargoWiseNetLibrary.Models.Native.cs in directory" -ForegroundColor Red
        Write-Host "  Searched for:" -ForegroundColor Yellow
        foreach ($file in $possibleFiles) {
            Write-Host "    - $(Split-Path $file -Leaf)" -ForegroundColor Yellow
        }
        exit 1
    }
} elseif (-not (Test-Path $FilePath -PathType Leaf)) {
    Write-Host "Error: File not found: $FilePath" -ForegroundColor Red
    exit 1
}

Write-Host "Processing: $(Split-Path $FilePath -Leaf)" -ForegroundColor Cyan

$content = Get-Content -Path $FilePath -Raw -Encoding UTF8
$originalContent = $content
$changeCount = 0

foreach ($mapping in $TypeMappings) {
    $collection = $mapping.Collection
    $type = $mapping.Type
    $camelCase = Convert-ToCamelCase -PascalCase $collection

    # Fix private field declaration
    $privateFieldPattern = "private Collection<CriteriaGroupType> _$camelCase;"
    $privateFieldReplacement = "private Collection<$type> _$camelCase;"

    if ($content -match [regex]::Escape($privateFieldPattern)) {
        $content = $content -replace [regex]::Escape($privateFieldPattern), $privateFieldReplacement
        $changeCount++
        Write-Verbose "Fixed private field: _$camelCase"
    }

    # Fix public property type
    $propertyPattern = "public Collection<CriteriaGroupType> $collection"
    $propertyReplacement = "public Collection<$type> $collection"

    if ($content -match [regex]::Escape($propertyPattern)) {
        $content = $content -replace [regex]::Escape($propertyPattern), $propertyReplacement
        $changeCount++
        Write-Verbose "Fixed property: $collection"
    }

    # Fix XmlArrayItemAttribute - match both CriteriaGroup and potential partial fixes
    # Pattern 1: Full wrong pattern (CriteriaGroup + CriteriaGroupType)
    $xmlAttrPattern1 = "\[XmlArrayItemAttribute\(`"CriteriaGroup`", Namespace=`"http://www\.cargowise\.com/Schemas/Native`"\)\]\s+public Collection<CriteriaGroupType> $collection"

    # Pattern 2: Partial fix (CriteriaGroup attribute but correct type)
    $xmlAttrPattern2 = "\[XmlArrayItemAttribute\(`"CriteriaGroup`", Namespace=`"http://www\.cargowise\.com/Schemas/Native`"\)\]\s+public Collection<$type> $collection"

    # Pattern 3: Wrong namespace with correct or wrong type
    $xmlAttrPattern3 = "\[XmlArrayItemAttribute\(`"CriteriaGroup`", Namespace=`"[^`"]+`"\)\]\s+public Collection<[^>]+> $collection"

    $xmlAttrReplacement = "[XmlArrayItemAttribute(`"$type`", Namespace=`"$Namespace`")]`r`n        public Collection<$type> $collection"

    $matched = $false
    foreach ($pattern in @($xmlAttrPattern1, $xmlAttrPattern2, $xmlAttrPattern3)) {
        if ($content -match $pattern) {
            $content = $content -replace $pattern, $xmlAttrReplacement
            $changeCount++
            Write-Verbose "Fixed XmlArrayItemAttribute: $collection"
            $matched = $true
            break
        }
    }
}

if ($content -ne $originalContent) {
    $content | Set-Content -Path $FilePath -Encoding UTF8 -NoNewline
    Write-Host "✓ Fixed $changeCount type references in $(Split-Path $FilePath -Leaf)" -ForegroundColor Green
    exit 0
} else {
    Write-Host "ℹ No changes needed in $(Split-Path $FilePath -Leaf)" -ForegroundColor Yellow
    exit 0
}
