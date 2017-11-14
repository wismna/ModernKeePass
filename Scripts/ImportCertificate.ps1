# Enable -Verbose option
[CmdletBinding()]

param(
    [string]$CertPath = "$Env:BUILD_ARTIFACTSTAGINGDIRECTORY",
    [Parameter(Mandatory=$true)][string]$FileName
)
Import-Certificate -Filepath "$CertPath\$FileName"