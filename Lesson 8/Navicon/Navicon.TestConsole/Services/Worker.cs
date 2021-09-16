using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Navicon.TestConsole.Services
{
    public class Worker : BackgroundService
    {
        private readonly IRandomLogger _randomLogger;

        public Worker(IRandomLogger randomLogger)
        {
            _randomLogger = randomLogger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine(_randomLogger.Log());
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}