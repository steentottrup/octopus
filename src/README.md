# Some step templates used with "Run a script"

## getcertthumbprint.csx

A C# script to the the thumbprint of a (webhosting) certificate. Used together with `windowsacmeclient.ps1` and the build-in "Deploy to IIS" to add a SSL certificate to a site, as "Deploy to IIS" needs the thumbprint of the certificate as an input argument.

## ghostinspector.csx

A C# script that will start a test suite on Ghost Inspector and wait for it to finish.

## windowsacmeclient.ps1

A PowerShell script that will request a Let's Encrypt certificate for a given domain.
