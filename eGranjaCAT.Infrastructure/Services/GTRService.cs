using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.GTR.Guies;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;


namespace eGranjaCAT.Infrastructure.Services
{
    public class GTRService : IGTRService
    {
        private readonly ILogger<GTRService> _logger;
        private readonly HttpClient _gtrClient;

        public GTRService(ILogger<GTRService> logger)
        {
            _logger = logger;
            _gtrClient = new HttpClient
            {
                BaseAddress = new Uri("https://preproduccio.aplicacions.agricultura.gencat.cat/gtr/")
            };
        }

        public async Task<ServiceResult<LoadDSTGuidesResponseDTO>> LoadAndGetDSTGuides(LoadDSTGuidesRequestDTO requestDTO)
        {
            try
            {
                var response = await _gtrClient.PostAsJsonAsync("WSMobilitat/AppJava/WSCarregaGuiesMobilitat", requestDTO);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<LoadDSTGuidesResponseDTO>();

                    if (responseData != null && responseData.Guias != null)
                    {
                        return ServiceResult<LoadDSTGuidesResponseDTO>.Ok(responseData, 200);
                    }
                    else
                    {
                        return ServiceResult<LoadDSTGuidesResponseDTO>.Fail("Resposta del servei GTR buida o no vàlida", 500);
                    }
                }
                else
                {
                    return ServiceResult<LoadDSTGuidesResponseDTO>.Fail($"Error HTTP: {response.StatusCode} - {response.ReasonPhrase}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cridar el servei GTR WSCarregaGuiesMobilitat");

                return ServiceResult<LoadDSTGuidesResponseDTO>.FromException(ex, 500);
            }
        }


        public async Task<ServiceResult<bool>> UpdateDSTGuides(UpdateDSTGuidesRequest requestDTO)
        {
            try
            {
                var response = await _gtrClient.PostAsJsonAsync("WSMobilitat/AppJava/WSModificarGuiasMovilitat", requestDTO);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<GTRSuccessResponseDTO>();

                    if (responseData?.Codi == "OK") return ServiceResult<bool>.Ok(true, 204);
                    else return ServiceResult<bool>.Fail($"Error en la resposta del servei GTR: {responseData?.Descripcio ?? "Resposta buida"}", 500);
                }
                else
                {
                    return ServiceResult<bool>.Fail($"Error HTTP: {response.StatusCode} - {response.ReasonPhrase}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cridar el servei GTR WSModificarGuiasMovilitat");

                return ServiceResult<bool>.FromException(ex, 500);
            }
        }
    }
}