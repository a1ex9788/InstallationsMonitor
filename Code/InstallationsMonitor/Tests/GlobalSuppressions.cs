using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "It is not possible since it is a configuration method for tests.", Scope = "member", Target = "~M:InstallationsMonitor.Tests.Utilities.ServiceProviders.MonitorCommandTestServiceProvider.#ctor")]