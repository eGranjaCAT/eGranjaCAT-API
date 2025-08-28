using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Lot;

namespace eGranjaCAT.Infrastructure.Services
{
    public interface ILotService
    {
        Task<ServiceResult<int?>> CreateLotAsync(int farmId, string userId, CreateLotDTO createLotDTO);
        Task<ServiceResult<bool>> DeleteLotAsync(int lotId);
        Task<MemoryStream> ExportLotByIdAsync(int lotId);
        Task<MemoryStream> ExportLotsAsync(int? pageIndex, int? pageSize);
        Task<MemoryStream> ExportActiveLotsAsync(int? pageIndex, int? pageSize);
        Task<MemoryStream> ExportLotsByFarmAsync(int farmId, int? pageIndex, int? pageSize);
        Task<MemoryStream> ExportActiveLotsByFarmAsync(int farmId, int? pageIndex, int? pageSize);
        Task<ServiceResult<PagedResult<GetLotDTO>>> GetActiveLotsByFarmAsync(int farmId, int pageIndex, int pageSize);
        Task<ServiceResult<GetLotDTO>> GetLotByIdAsync(int lotId);
        Task<ServiceResult<PagedResult<GetLotDTO>>> GetLotsByFarmIdAsync(int farmId, int pageIndex, int pageSize);
        Task<ServiceResult<bool>> UpdateLotAsync(int lotId, UpdateLotDTO dto);
    }
}