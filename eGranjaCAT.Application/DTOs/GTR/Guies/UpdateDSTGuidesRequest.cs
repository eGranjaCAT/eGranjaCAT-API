using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace eGranjaCAT.Application.DTOs.GTR.Guies
{
    public class UpdateDSTGuidesRequest
    {
        [Required, StringLength(9, MinimumLength = 9)]
        [JsonPropertyName("NIF")]
        public required string NIF { get; set; }

        [Required]
        [JsonPropertyName("PASSWORD_MOBILITAT")]
        public required string PASSWORD_MOBILITAT { get; set; }

        [Required]
        [JsonPropertyName("GUIAS")]
        public List<DSTGuideDTO> Guias { get; set; } = [];
    }
}