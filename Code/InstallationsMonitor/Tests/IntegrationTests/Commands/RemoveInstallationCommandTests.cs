﻿using FluentAssertions;
using InstallationsMonitor.Domain;
using InstallationsMonitor.Persistence;
using InstallationsMonitor.ServiceProviders.Base;
using InstallationsMonitor.Tests.Utilities.ServiceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace InstallationsMonitor.Tests.IntegrationTests.Commands
{
    [TestClass]
    public class RemoveInstallationCommandTests
    {
        [TestMethod]
        public void RemoveCommand_ExistentIdentifier_RemovesInstallation()
        {
            // Arrange.
            Installation installation = new Installation("Program", new DateTime(1, 1, 1, 1, 1, 1))
            {
                Id = 1,
            };

            string[] args = new string[] { "remove", "-i", installation.Id.ToString() };

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IServiceProvider serviceProvider = new RemoveCommandTestServiceProvider(
                cancellationTokenSource.Token);
            AppDbContext appDbContext = serviceProvider.GetRequiredService<AppDbContext>();

            appDbContext.Installations.Add(installation);
            appDbContext.SaveChanges();

            CommandsServiceProvider.ExtraRegistrationsAction =
                sc => sc.AddSingleton(serviceProvider.GetRequiredService<DatabaseOptions>());

            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act.
            Program.Main(args);

            // Assert.
            stringWriter.ToString().Should().Be(
                $"Installation with id '{installation.Id}' removed.{Environment.NewLine}");

            appDbContext.Installations.Should().BeEmpty();
        }

        [TestMethod]
        public void RemoveCommand_WithoutIdentifier_ThrowsException()
        {
            // Arrange.
            string[] args = new string[] { "remove" };

            using StringWriter outStringWriter = new StringWriter();
            Console.SetOut(outStringWriter);

            using StringWriter errorStringWriter = new StringWriter();
            Console.SetError(errorStringWriter);

            // Act.
            Program.Main(args);

            // Assert.
            outStringWriter.ToString().Should().BeEmpty();

            errorStringWriter.ToString().Should().Contain(
                $"The -i field is required.{Environment.NewLine}");
        }

        [TestMethod]
        public void RemoveCommand_NotIntegerIdentifier_ThrowsException()
        {
            // Arrange.
            string[] args = new string[] { "remove", "-i", "notInteger" };

            using StringWriter outStringWriter = new StringWriter();
            Console.SetOut(outStringWriter);

            using StringWriter errorStringWriter = new StringWriter();
            Console.SetError(errorStringWriter);

            // Act.
            Program.Main(args);

            // Assert.
            outStringWriter.ToString().Should().BeEmpty();

            errorStringWriter.ToString().Should().Contain("The -i field must be an integer.");
        }
    }
}