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
            var resultObj = new ServiceResult<bool>();
            try
            {
                var result = await _presvetClient.PostAsJsonAsync("login/authenticate", new { username, password });
                if (!result.IsSuccessStatusCode)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add($"Error HTTP: {result.StatusCode} - {result.ReasonPhrase}");
                    return resultObj;
                }

                var token = await result.Content.ReadFromJsonAsync<string>();

                if (string.IsNullOrEmpty(token))
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("Resposta buida del servei Presvet");
                    return resultObj;
                }

                _presvetClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                resultObj.Success = true;
                return resultObj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al connectar amb el servei Presvet");
                resultObj.Success = false;
                resultObj.Errors.Add("Error inesperat en connectar amb el servei Presvet");
                return resultObj;
            }
        }


        // en els docs no s'especifica que retorna !!! 
        public async Task<ServiceResult<bool>> CheckPresvetConnection()
        {
            var resultObj = new ServiceResult<bool>();
            try
            {
                var result = await _presvetClient.GetAsync("comunicacionprescripcion/estaactivo");

                if (!result.IsSuccessStatusCode)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add($"Error HTTP: {result.StatusCode} - {result.ReasonPhrase}");
                    return resultObj;
                }

                var isActive = await result.Content.ReadFromJsonAsync<bool>();
                if (!isActive)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("El servei Presvet no està actiu");
                    return resultObj;
                }

                resultObj.Success = true;
                return resultObj;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al comprovar la connexió amb el servei Presvet");
                resultObj.Success = false;
                resultObj.Errors.Add("Error inesperat en comprovar la connexió amb el servei Presvet");
                return resultObj;
            }
        }
    }
}