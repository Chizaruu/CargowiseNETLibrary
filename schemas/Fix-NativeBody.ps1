#Requires -Version 5.0

<#
.SYNOPSIS
    Fixes NativeBody collections - SMART version that uses context.

.PARAMETER FilePath
    Path to the Native.cs file to fix.
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

$TypeMappings = @{
    'Product' = 'ProductData'
    'Organization' = 'OrganizationData'
    'Company' = 'CompanyData'
    'Container' = 'ContainerData'
    'Country' = 'CountryData'
    'Airline' = 'AirlineData'
    'Vessel' = 'VesselData'
    'Rate' = 'RateData'
    'Staff' = 'StaffData'
    'Tag' = 'TagData'
    'TagRule' = 'TagRuleData'
    'CommodityCode' = 'CommodityCodeData'
    'DangerousGood' = 'DangerousGoodData'
    'ServiceLevel' = 'ServiceLevelData'
    'UNLOCO' = 'UNLOCOData'
    'CurrencyExchangeRate' = 'CurrencyExchangeRateData'
    'CusStatement' = 'CusStatementData'
    'BMSystem' = 'BMSystemData'
    'AcceptabilityBand' = 'AcceptabilityBandData'
    'WorkflowTemplate' = 'WorkflowTemplateData'
}

$Namespace = 'http://www.cargowise.com/Schemas/Native/2011/11'

function Convert-ToCamelCase {
    param([string]$str)
    return $str.Substring(0, 1).ToLowerInvariant() + $str.Substring(1)
}

Write-Host "Processing: $(Split-Path $FilePath -Leaf)" -ForegroundColor Cyan

$content = Get-Content -Path $FilePath -Raw -Encoding UTF8
$changeCount = 0

# PASS 1: Fix fields, properties, and constructors
Write-Host "Pass 1: Fixing fields, properties, and constructors..." -ForegroundColor Cyan

foreach ($collectionName in $TypeMappings.Keys) {
    $wrapperType = $TypeMappings[$collectionName]
    $camelCase = Convert-ToCamelCase -str $collectionName

    # 1. Fix private field
    $oldField = "private Collection<CriteriaGroupType> _$camelCase;"
    $newField = "private Collection<$wrapperType> _$camelCase;"
    if ($content.Contains($oldField)) {
        $content = $content.Replace($oldField, $newField)
        $changeCount++
    }

    # 2. Fix public property
    $oldProp = "public Collection<CriteriaGroupType> $collectionName"
    $newProp = "public Collection<$wrapperType> $collectionName"
    if ($content.Contains($oldProp)) {
        $content = $content.Replace($oldProp, $newProp)
        $changeCount++
    }

    # 3. Fix constructor
    $oldCtor = "this._$camelCase = new Collection<CriteriaGroupType>();"
    $newCtor = "this._$camelCase = new Collection<$wrapperType>();"
    if ($content.Contains($oldCtor)) {
        $content = $content.Replace($oldCtor, $newCtor)
        $changeCount++
    }
}

# PASS 2: Fix XmlArrayItemAttribute by looking at the property type that follows it
Write-Host "Pass 2: Fixing XmlArrayItemAttribute..." -ForegroundColor Cyan

$badAttr = '[XmlArrayItemAttribute("CriteriaGroup", Namespace="http://www.cargowise.com/Schemas/Native")]'
$searchPos = 0

while (($searchPos = $content.IndexOf($badAttr, $searchPos)) -ge 0) {
    # Found a bad attribute - look ahead to find the property
    # Look for: public Collection<WrapperType>
    $lookAheadStart = $searchPos + $badAttr.Length
    $lookAheadEnd = [Math]::Min($lookAheadStart + 200, $content.Length)
    $lookAheadText = $content.Substring($lookAheadStart, $lookAheadEnd - $lookAheadStart)

    # Extract the wrapper type from "public Collection<WrapperType>"
    if ($lookAheadText -match 'public Collection<(\w+)>') {
        $wrapperType = $matches[1]

        # Replace this specific occurrence
        $newAttr = "[XmlArrayItemAttribute(`"$wrapperType`", Namespace=`"$Namespace`")]"
        $content = $content.Remove($searchPos, $badAttr.Length)
        $content = $content.Insert($searchPos, $newAttr)

        $changeCount++
        Write-Verbose "Fixed attribute -> $wrapperType"

        # Move past this replacement
        $searchPos = $searchPos + $newAttr.Length
    } else {
        # Couldn't find property, skip this one
        $searchPos = $searchPos + $badAttr.Length
    }
}

# PASS 3: Fix Tag/TagRule swap (using regex for robustness)
Write-Host "Pass 3: Checking for Tag/TagRule swap..." -ForegroundColor Cyan

# Fix private fields
if ($content -match 'private\s+Collection<TagRuleData>\s+_tag;') {
    Write-Host "Detected _tag field with wrong type, fixing..." -ForegroundColor Yellow
    $content = $content -replace 'private\s+Collection<TagRuleData>\s+_tag;', 'private Collection<TagData> _tag;'
    $changeCount++
    Write-Host "Fixed _tag field declaration" -ForegroundColor Green
}

if ($content -match 'private\s+Collection<TagData>\s+_tagRule;') {
    Write-Host "Detected _tagRule field with wrong type, fixing..." -ForegroundColor Yellow
    $content = $content -replace 'private\s+Collection<TagData>\s+_tagRule;', 'private Collection<TagRuleData> _tagRule;'
    $changeCount++
    Write-Host "Fixed _tagRule field declaration" -ForegroundColor Green
}

# Fix property declarations
if ($content -match 'public\s+Collection<TagRuleData>\s+Tag\s') {
    Write-Host "Detected Tag property with wrong type, fixing..." -ForegroundColor Yellow
    $content = $content -replace 'public\s+Collection<TagRuleData>\s+Tag\s', 'public Collection<TagData> Tag '
    $changeCount++
    Write-Host "Fixed Tag property declaration" -ForegroundColor Green
}

if ($content -match 'public\s+Collection<TagData>\s+TagRule\s') {
    Write-Host "Detected TagRule property with wrong type, fixing..." -ForegroundColor Yellow
    $content = $content -replace 'public\s+Collection<TagData>\s+TagRule\s', 'public Collection<TagRuleData> TagRule '
    $changeCount++
    Write-Host "Fixed TagRule property declaration" -ForegroundColor Green
}

# Fix constructor
if ($content -match 'this\._tag\s*=\s*new\s+Collection<TagRuleData>\(\s*\)\s*;') {
    Write-Host "Detected Tag constructor with wrong type, fixing..." -ForegroundColor Yellow
    $content = $content -replace 'this\._tag\s*=\s*new\s+Collection<TagRuleData>\(\s*\)\s*;', 'this._tag = new Collection<TagData>();'
    $changeCount++
    Write-Host "Fixed Tag constructor" -ForegroundColor Green
}

if ($content -match 'this\._tagRule\s*=\s*new\s+Collection<TagData>\(\s*\)\s*;') {
    Write-Host "Detected TagRule constructor with wrong type, fixing..." -ForegroundColor Yellow
    $content = $content -replace 'this\._tagRule\s*=\s*new\s+Collection<TagData>\(\s*\)\s*;', 'this._tagRule = new Collection<TagRuleData>();'
    $changeCount++
    Write-Host "Fixed TagRule constructor" -ForegroundColor Green
}

# Save the file
Set-Content -Path $FilePath -Value $content -Encoding UTF8 -NoNewline

$fileName = Split-Path $FilePath -Leaf
Write-Host "Fixed $changeCount type references in $fileName" -ForegroundColor Green

exit 0
