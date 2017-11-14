# Enable -Verbose option
[CmdletBinding()]

param(
    [Parameter(Mandatory=$true)][string]$CertPath
)
Write-Host "Registering certificate: $CertPath" -CertStoreLocation cert:\CurrentUser\Root