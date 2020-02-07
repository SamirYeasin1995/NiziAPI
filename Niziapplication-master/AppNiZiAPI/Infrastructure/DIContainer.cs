using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace AppNiZiAPI.Infrastructure
{
    public sealed class DIContainer
    {
        private static readonly IServiceProvider _instance = Build();
        public static IServiceProvider Instance => _instance;

        static DIContainer()
        {

        }

        private DIContainer()
        {

        }

        private static IServiceProvider Build()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddRepositoryServices();
            services.AddAPIServices();
            services.AddHelperServices();

            return services.BuildServiceProvider();
        }
    }
}
