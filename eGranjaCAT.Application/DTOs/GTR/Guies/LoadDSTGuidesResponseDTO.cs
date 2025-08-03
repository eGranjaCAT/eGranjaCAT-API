using System.Text.Json.Serialization;


namespace eGranjaCAT.Application.DTOs.GTR.Guies
{
    public class LoadDSTGuidesResponseDTO
    {
        [JsonPropertyName("GUIAS")]
        public List<DSTGuideDTO> Guias { get; set; } = [];
    }
}