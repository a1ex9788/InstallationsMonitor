using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace InstallationsMonitor.Commands
{
    internal abstract class Command : ICommand
    {
        public async Task ExecuteAsync()
        {
            IServiceProvider serviceProvider = this.BuildServices();

            await this.ExecuteAsync(serviceProvider);
        }

        protected abstract Task ExecuteAsync(IServiceProvider serviceProvider);

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