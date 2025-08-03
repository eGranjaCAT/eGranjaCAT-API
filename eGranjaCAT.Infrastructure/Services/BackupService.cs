using Microsoft.Extensions.Logging;
using eGranjaCAT.Application.Common;


namespace eGranjaCAT.Infrastructure.Services
{
    public class BackupService : IBackupService
    {
        private readonly ILogger<BackupService> logger;

        public BackupService(ILogger<BackupService> logger)
        {
            this.logger = logger;
        }

        public async Task<ServiceResult<string>> CreateAndSendBackupAsync()
        {
            return null;
        }
    }
}
