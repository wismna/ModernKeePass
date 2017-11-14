# Enable -Verbose option
[CmdletBinding()]

param(
    [string]$CertPath = "$Env:BUILD_ARTIFACTSTAGINGDIRECTORY",
    [Parameter(Mandatory=$true)][string]$FileName
)
Write-Host "Registering certificate: $CertPath\$FileName"
Import-Certificate -Filepath "$CertPath\$FileName"