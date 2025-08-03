using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.GTR.Guies;

namespace eGranjaCAT.Infrastructure.Services
{
    public interface IGTRService
    {
        Task<ServiceResult<LoadDSTGuidesResponseDTO>> LoadAndGetDSTGuides(LoadDSTGuidesRequestDTO requestDTO);
        Task<ServiceResult<bool>> UpdateDSTGuides(UpdateDSTGuidesRequest requestDTO);
    }
}