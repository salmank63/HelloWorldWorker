using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelloWorldWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient httpClient;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            httpClient = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            httpClient.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var admin = await httpClient.GetAsync("https://admin.jugaadhai.in/api/Article/AppArticleListLazyLoad?skipNumber=0");
                var sample = await httpClient.GetAsync("http://sample.jitangupta.com/api/Article/AppArticleListLazyLoad?skipNumber=0");

                if (!admin.IsSuccessStatusCode || !sample.IsSuccessStatusCode)
                {
                    continue;
                }

                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(60 * 4 * 1000, stoppingToken);
            }
        }
    }
}
