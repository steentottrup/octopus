Write-Host "Getting cert for: " $Param_Domain
Write-Host "Site is in: " $Param_Path

$acme = "c:\tools\letsencrypt\wacs.exe"
$Params = @(
    "--target"
    "manual"
    "--host"
    $Param_Domain
    "--webroot" 
    $Param_Path
    "--emailaddress"
    "info@skolevisioner.dk"
    "--accepttos"
    "--usedefaulttaskuser"
)
& $acme $Params