using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SerenBot.Models;
using SerenBot.Services;
using SerenBot.Services.Interfaces;

namespace SerenBot.HostedServices
{
    public class VosScraperService : IHostedService, IDisposable
    {
        public static VoiceOfSeren[] CurrentVos = new VoiceOfSeren[2];

        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private Timer _timer;

        public VosScraperService(
            IConfiguration configuration,
            ILogger<VosScraperService> logger,
            IServiceProvider services
        )
        {
            _config = configuration;
            _logger = logger;
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Vos Scraper Service is starting");

            _timer = new Timer(CheckVOS, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Vos Scraper Service is stopping");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void CheckVOS(object state)
        {
            CheckVOSAsync().GetAwaiter().GetResult();
        }

        private async Task CheckVOSAsync()
        {
            _logger.LogInformation("Running Voice of seren update.");

            IVosService vosService = _services.GetService<IVosService>();
            VoiceOfSeren[] voices = await vosService.GetActiveVos();
            if (!voices.SequenceEqual(CurrentVos))
            {
                CurrentVos = voices;
                var notifier = _services.GetService<IDiscordGuildNotifier>();
                await notifier.SendVosNotifications(voices);

                _logger.LogInformation($"The current voices of seren are {voices[0]} and {voices[1]}");
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
