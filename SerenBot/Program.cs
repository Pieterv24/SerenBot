using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SerenBot.Entities;
using SerenBot.HostedServices;
using SerenBot.Services;
using SerenBot.Services.Interfaces;

namespace SerenBot
{
    class Program
    {
        private static string env;
        private static IConfiguration Configuration;

        static void Main(string[] args)
        {
            env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrWhiteSpace(env))
            {
                env = "Development";
            }

            var host = new HostBuilder()
                .UseEnvironment(string.IsNullOrWhiteSpace(env) ? EnvironmentName.Production : EnvironmentName.Development)
                .ConfigureHostConfiguration(GetConfigureDelegate)
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .Build();

            ApplyDatabaseMigrations(host.Services);

            host.RunAsync().GetAwaiter().GetResult();
        }

        private static void GetConfigureDelegate(IConfigurationBuilder builder)
        {
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddLogging();

            string DBType = Configuration.GetValue<string>("Database:DBType");

            switch(DBType.ToUpper())
            {
                case "MYSQL":
                    serviceCollection.AddDbContextPool<SerenDbContext>(options =>
                        options.UseMySql(Configuration.GetValue<string>("Database:ConnectionString")));
                    break;
                case "SQLITE":
                default:
                    serviceCollection.AddDbContextPool<SerenDbContext>(options =>
                        options.UseSqlite(Configuration.GetValue<string>("Database:ConnectionString")));
                    break;
            }

            serviceCollection.AddTransient<IAuth, TwitterAuth>();
            serviceCollection.AddTransient<IVosService, VosService>();
            serviceCollection.AddTransient<IDiscordGuildNotifier, DiscordGuildNotifier>();

            serviceCollection.AddHostedService<VosScraperService>();
            serviceCollection.AddHostedService<DiscordBotService>();
        }

        public static void ApplyDatabaseMigrations(IServiceProvider services)
        {
            SerenDbContext context = services.GetService<SerenDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
