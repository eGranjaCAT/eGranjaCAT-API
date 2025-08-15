using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Farm;

namespace eGranjaCAT.Infrastructure.Services
{
    public interface IFarmService
    {
        Task<ServiceResult<int?>> CreateFarmAsync(CreateFarmDTO createFarmDTO, string userId);
        Task<ServiceResult<bool>> DeleteFarmAsync(int id);
        Task<ServiceResult<GetFarmDTO?>> GetFarmByIdAsync(int id);
        Task<ServiceResult<PagedResult<GetFarmDTO>>> GetFarmsAsync(int pageIndex, int pageSize);
    }
}