using Microsoft.Extensions.DependencyInjection;
using System;

namespace InstallationsMonitor.Commands
{
    internal abstract class Command
    {
        internal void Execute()
        {
            IServiceProvider serviceProvider = this.BuildServices();

            this.Execute(serviceProvider);
        }

        protected abstract void Execute(IServiceProvider serviceProvider);

        protected virtual void ConfigureSpecificServices(IServiceCollection services)
        {
        }

        private IServiceProvider BuildServices()
        {
            IServiceCollection services = new ServiceCollection();

            this.ConfigureSpecificServices(services);

            return services.BuildServiceProvider();
        }
    }
}