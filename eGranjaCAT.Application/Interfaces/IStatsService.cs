
namespace eGranjaCAT.Infrastructure.Services
{
    public interface IStatsService
    {
        Task<int> CountRecentRecordsAsync(int days);
        Task<int> CountTotalActiveLotsAsync();
        Task<int> CountTotalFarmsAsync();
        Task<int> CountTotalRecordsAsync();
        Task<int> CountTotalUsersAsync();
    }
}