var domain = Octopus.Parameters["Param_Domain"].ToLower();
Console.WriteLine($"Looking for certificate for {domain}");

var store = new System.Security.Cryptography.X509Certificates.X509Store("WebHosting", System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine);
store.Open(System.Security.Cryptography.X509Certificates.OpenFlags.ReadOnly);

foreach (System.Security.Cryptography.X509Certificates.X509Certificate2 mCert in store.Certificates) {
  if (domain == mCert.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.DnsName, false)) {
    Console.WriteLine($"Found cert, thumbprint is {mCert.Thumbprint}!");
    Octopus.SetVariable("Thumbprint", mCert.Thumbprint);
    break;
  }
}
