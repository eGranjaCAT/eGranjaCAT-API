using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Entrada;

namespace eGranjaCAT.Infrastructure.Services
{
    public interface IEntradaService
    {
        Task<ServiceResult<int?>> CreateEntradaAsync(int farmId, string userId, CreateEntradaDTO createEntradaDTO);
        Task<ServiceResult<bool>> DeleteEntradaAsync(int farmId, int entradaId);
        Task<MemoryStream> ExportEntradaByIdAsync(int entradaId);
        Task<MemoryStream> ExportEntradesAsync(int? pageIndex, int? pageSize);
        Task<MemoryStream> ExportEntradesByFarmAsync(int farmId, int? pageIndex, int? pageSize);
        Task<ServiceResult<GetEntradaDTO?>> GetEntradaByIdAsync(int entradaId);
        Task<ServiceResult<PagedResult<GetEntradaDTO>>> GetEntradesAsync(int farmId, int pageIndex, int pageSize);
        Task<ServiceResult<bool>> UpdateEntradaAsync(int farmId, int entradaId, UpdateEntradaDTO updateEntradaDTO);
    }
}