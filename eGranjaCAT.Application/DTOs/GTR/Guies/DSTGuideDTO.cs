using eGranjaCAT.Shared.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace eGranjaCAT.Application.DTOs.GTR.Guies
{
    public class DSTGuideDTO
    {
        [Required, StringLength(6, MinimumLength = 6)]
        [JsonPropertyName("CODI_MO")]
        public required string CodiMo { get; set; }

        [Required, StringLength(20, MinimumLength = 1)]
        [JsonPropertyName("REMO")]
        public required string Remo { get; set; }

        [Required, StringLength(6, MinimumLength = 6)]
        [JsonPropertyName("MO_DESTI")]
        public required string MoDesti { get; set; }

        [Required, RegularExpression("^(00|01|02|03|04|05)$")]
        [JsonPropertyName("CATEGORIA")]
        public required string Categoria { get; set; }

        [Required, Range(1, int.MaxValue)]
        [JsonPropertyName("NOMBRE_ANIMALS")]
        public required int NombreAnimals { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [JsonPropertyName("TRANSPORTISTA")]
        public string? Transportista { get; set; }

        [StringLength(20, MinimumLength = 1)]
        [JsonPropertyName("RESPONSABLE")]
        public string? Responsable { get; set; }

        [StringLength(20, MinimumLength = 1)]
        [JsonPropertyName("VEHICLE")]
        public string? Vehicle { get; set; }

        [Required, RegularExpression(@"\d{12}", ErrorMessage = "Formato: yyyymmddHHMM")]
        [JsonPropertyName("DATA_SORTIDA")]
        [CustomValidation(typeof(ValidationsUtils), nameof(ValidationsUtils.ValidateDate))]
        public required string DataSortida { get; set; }

        [Required, RegularExpression(@"\d{12}", ErrorMessage = "Formato: yyyymmddHHMM")]
        [JsonPropertyName("DATA_ARRIBADA")]
        [CustomValidation(typeof(ValidationsUtils), nameof(ValidationsUtils.ValidateDate))]
        public required string DataArribada { get; set; }
    }
}