using eGranjaCAT.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace eGranjaCAT.Infrastructure.Extensions.Cron.Jobs
{
    public class BackupJob : CronBackgroundJob
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BackupJob(CronSettings<BackupJob> settings, ILogger<BackupJob> logger, IServiceScopeFactory scopeFactory
        ) : base(settings.CronExpression, settings.TimeZone, logger)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task DoWork(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var backupService = scope.ServiceProvider.GetRequiredService<IBackupService>();

            _logger.LogInformation("Starting backup at {Time}", DateTime.UtcNow);
            var result = await backupService.CreateAndSendBackupAsync();
        }
    }
}