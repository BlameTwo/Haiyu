using System.Runtime.Versioning;

// Haiyu is a Windows-only WinUI application. Declaring the supported platform at
// assembly level prevents CA1416 from treating every Windows API call as reachable
// from other operating systems.
[assembly: SupportedOSPlatform("windows10.0.17763.0")]
