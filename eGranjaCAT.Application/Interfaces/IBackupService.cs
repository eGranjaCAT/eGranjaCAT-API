using eGranjaCAT.Application.Common;

namespace eGranjaCAT.Infrastructure.Services
{
    public interface IBackupService
    {
        Task<ServiceResult<string>> CreateAndSendBackupAsync();
    }
}