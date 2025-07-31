using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eGranjaCAT.Infrastructure.Extensions.Cron
{
    public abstract class CronBackgroundJob : BackgroundService
    {
        private readonly CronExpression _cronExpression;
        private readonly TimeZoneInfo _timeZone;
        protected readonly ILogger _logger;

        protected CronBackgroundJob(string rawCronExpression, TimeZoneInfo timeZone, ILogger logger)
        {
            _cronExpression = CronExpression.Parse(rawCronExpression);
            _timeZone = timeZone;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTimeOffset.UtcNow;
                var next = _cronExpression.GetNextOccurrence(now, _timeZone);
                if (next == null) return;

                var delay = next.Value - now;
                if (delay.TotalMilliseconds > 0)
                    await Task.Delay(delay, stoppingToken);

                try
                {
                    await DoWork(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al ejecutar el cron job.");
                }
            }
        }

        protected abstract Task DoWork(CancellationToken stoppingToken);
    }
}
