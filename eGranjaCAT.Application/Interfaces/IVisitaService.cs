using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Visites;

namespace eGranjaCAT.Infrastructure.Services
{
    public interface IVisitaService
    {
        Task<ServiceResult<int?>> CreateVisitaAsync(int farmId, string userId, CreateVisitaDTO createVisitaDTO);
        Task<ServiceResult<bool>> DeleteVisitaAsync(int farmId, int visitaId);
        Task<MemoryStream> ExportVisitesAsync();
        Task<MemoryStream> ExportVisitesByFarmAsync(int farmId);
        Task<MemoryStream> ExportVisitesByIdAsync(int visitaId);
        Task<ServiceResult<GetVisitaDTO?>> GetVisitaByIdAsync(int visitaId);
        Task<ServiceResult<List<GetVisitaDTO>>> GetVisitesAsync(int farmId);
        Task<ServiceResult<bool>> UpdateVisitaAsync(int farmId, int visitaId, UpdateVisitaDTO updateVisitaDTO);
    }
}