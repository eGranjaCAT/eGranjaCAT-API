using System.Text.Json.Serialization;


namespace eGranjaCAT.Application.DTOs.GTR.Guies
{
    public class GTRSuccessResponseDTO
    {
        [JsonPropertyName("codi")]
        public required string Codi { get; set; }

        [JsonPropertyName("descripcio")]
        public required string Descripcio { get; set; }
    }
}