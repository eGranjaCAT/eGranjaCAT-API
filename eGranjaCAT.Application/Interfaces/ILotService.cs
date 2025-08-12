using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Lot;

namespace eGranjaCAT.Infrastructure.Services
{
    public interface ILotService
    {
        Task<ServiceResult<int?>> CreateLotAsync(int farmId, string userId, CreateLotDTO createLotDTO);
        Task<ServiceResult<bool>> DeleteLotAsync(int lotId);
        Task<MemoryStream> ExportLotByIdAsync(int lotId);
        Task<MemoryStream> ExportLotsAsync();
        Task<MemoryStream> ExportActiveLotsAsync();
        Task<MemoryStream> ExportLotsByFarmAsync(int farmId);
        Task<MemoryStream> ExportActiveLotsByFarmAsync(int farmId);
        Task<ServiceResult<PagedResult<GetLotDTO>>> GetActiveLotsByFarmAsync(int farmId, int pageIndex, int pageSize);
        Task<ServiceResult<GetLotDTO>> GetLotByIdAsync(int lotId);
        Task<ServiceResult<GetLotDTO>> GetLotsByFarmIdAsync(int farmId);
        Task<ServiceResult<bool>> UpdateLotAsync(int farmId, int lotId, UpdateLotDTO dto);
    }
}