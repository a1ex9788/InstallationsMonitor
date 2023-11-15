using InstallationsMonitor.Entities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace InstallationsMonitor.Commands.Installations.Utilities
{
    internal class InstallationsPrinter
    {
        private readonly InstallationsObtainer installationsObtainer;
        private readonly CancellationToken cancellationToken;

        public InstallationsPrinter(
            InstallationsObtainer installationsObtainer, CancellationToken cancellationToken)
        {
            this.installationsObtainer = installationsObtainer;
            this.cancellationToken = cancellationToken;
        }

        internal void Print()
        {
            IEnumerable<Installation> installations = this.installationsObtainer.GetInstallations();

            foreach (Installation installation in installations)
            {
                if (this.cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                Console.WriteLine(installation.ProgramName + " - " + installation.DateTime);
            }
        }
    }
}