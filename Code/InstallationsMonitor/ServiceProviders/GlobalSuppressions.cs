using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1068:CancellationToken parameters must come last", Justification = "These methods configure the DI, they does not use the cancellation token.", Scope = "namespaceanddescendants", Target = "~N:InstallationsMonitor.ServiceProviders")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "It is a hook for tests.", Scope = "member", Target = "~F:InstallationsMonitor.ServiceProviders.Base.CommandsServiceProvider.ExtraRegistrationsAction")]