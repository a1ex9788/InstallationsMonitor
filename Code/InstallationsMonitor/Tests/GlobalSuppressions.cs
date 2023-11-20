using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1068:CancellationToken parameters must come last", Justification = "These methods configure the DI, they does not use the cancellation token.", Scope = "namespaceanddescendants", Target = "~N:InstallationsMonitor.Tests.Utilities.ServiceProviders")]