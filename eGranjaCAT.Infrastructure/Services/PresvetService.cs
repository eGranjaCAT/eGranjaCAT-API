using eGranjaCAT.Application.Common;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace eGranjaCAT.Infrastructure.Services
{
    public class PresvetService
    {
        private readonly ILogger<PresvetService> _logger;
        private readonly HttpClient _presvetClient;

        public PresvetService(ILogger<PresvetService> logger)
        {
            _logger = logger;

            _presvetClient = new HttpClient
            {
                BaseAddress = new Uri("https://integracion-servicio.mapa.gob.es/presvet/api/"),
            };
            _presvetClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<ServiceResult<bool>> ConnectToPresvetAsync(string username, string password)
        {
            try
            {
                var result = await _presvetClient.PostAsJsonAsync("login/authenticate", new { username, password });
                if (!result.IsSuccessStatusCode) return ServiceResult<bool>.Fail(new List<string> { $"Error HTTP: {result.StatusCode} - {result.ReasonPhrase}" }, (int)result.StatusCode);

                var token = await result.Content.ReadFromJsonAsync<string>();
                if (string.IsNullOrEmpty(token)) return ServiceResult<bool>.Fail(new List<string> { "No s'ha obtingut cap token d'autenticació" }, 500);

                _presvetClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return ServiceResult<bool>.Ok(true, 204);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al connectar amb el servei Presvet");

                return ServiceResult<bool>.FromException(ex, 500);
            }
        }


        // en els docs no s'especifica que retorna !!! 
        public async Task<ServiceResult<bool>> CheckPresvetConnection()
        {
            try
            {
                var result = await _presvetClient.GetAsync("comunicacionprescripcion/estaactivo");
                if (!result.IsSuccessStatusCode) return ServiceResult<bool>.Fail(new List<string> { $"Error HTTP: {result.StatusCode} - {result.ReasonPhrase}" }, (int)result.StatusCode);

                var isActive = await result.Content.ReadFromJsonAsync<bool>();
                if (!isActive) return ServiceResult<bool>.Fail(new List<string> { "El servei Presvet no està actiu" }, 503);

                return ServiceResult<bool>.Ok(true, 204);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al comprovar la connexió amb el servei Presvet");

                return ServiceResult<bool>.FromException(ex, 500);
            }
        }
    }
}