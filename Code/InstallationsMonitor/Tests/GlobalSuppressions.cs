using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "It is not important at tests.", Scope = "member", Target = "~M:InstallationsMonitor.Tests.Utilities.DatabaseUtilities.GetTestDatabaseConnection~InstallationsMonitor.Persistence.DatabaseConnection")]