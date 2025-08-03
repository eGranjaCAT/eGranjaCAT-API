using eGranjaCAT.Shared.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace eGranjaCAT.Application.DTOs.GTR.Guies
{
    public class LoadDSTGuidesRequestDTO
    {
        [Required, StringLength(9, MinimumLength = 9)]
        [JsonPropertyName("NIF")]
        public required string NIF { get; set; }

        [Required]
        [JsonPropertyName("PASSWORD_MOBILITAT")]
        public required string PASSWORD_MOBILITAT { get; set; }

        [Required, StringLength(6, MinimumLength = 6)]
        [JsonPropertyName("CODI_MO")]
        public required string CODI_MO { get; set; }

        [Required, StringLength(50, MinimumLength = 1)]
        [JsonPropertyName("CODI_REGA")]
        public required string CODI_REGA { get; set; }

        [Required, RegularExpression(@"\d{12}", ErrorMessage = "Formato: yyyymmddHHMM")]
        [JsonPropertyName("DATA_SORTIDA")]
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(ValidationsUtils), nameof(ValidationsUtils.ValidateDate))]
        public required string DATA_SORTIDA { get; set; }
    }
}