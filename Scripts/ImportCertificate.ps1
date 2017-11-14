# Enable -Verbose option
[CmdletBinding()]

param(
    [Parameter(Mandatory=$true)][string]$CertPath
)
Write-Host "Registering certificate: $CertPath"
Import-Certificate -Filepath "$CertPath" -CertStoreLocation cert:\CurrentUser\Root