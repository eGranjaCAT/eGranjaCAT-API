using eGranjaCAT.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace eGranjaCAT.Application.DTOs.Entrada
{
    public class UpdateEntradaDTO : IValidatableObject
    {
        [Required]
        public int FarmId { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CategoriesEnum Categoria { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required]
        public int NombreAnimals { get; set; }

        [Required]
        public double PesTotal { get; set; }

        [Required]
        public double PesIndividual { get; set; }

        [Required]
        public int LotId { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrigensEnum Origen { get; set; }

        public string? MarcaOficial { get; set; }

        public string? CodiREGA { get; set; }

        [Required]
        public string NumeroDocumentTrasllat { get; set; } = null!;

        public string? Observacions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (Origen)
            {
                case OrigensEnum.MarcaOficial:
                    if (string.IsNullOrWhiteSpace(MarcaOficial))
                    {
                        yield return new ValidationResult(
                            "Marca Oficial és obligatori quan Origen és Marca Oficial.",
                            new[] { nameof(MarcaOficial) });
                    }
                    if (!string.IsNullOrWhiteSpace(CodiREGA))
                    {
                        yield return new ValidationResult(
                            "Codi REGA no ha de tenir valor quan Origen és Marca Oficial.",
                            new[] { nameof(CodiREGA) });
                    }
                    break;

                case OrigensEnum.CodiREGA:
                    if (string.IsNullOrWhiteSpace(CodiREGA))
                    {
                        yield return new ValidationResult(
                            "Codi REGA és obligatori quan Origen és Codi REGA.",
                            new[] { nameof(CodiREGA) });
                    }
                    if (!string.IsNullOrWhiteSpace(MarcaOficial))
                    {
                        yield return new ValidationResult(
                            "Marca Oficial no ha de tenir valor quan Origen és Codi REGA.",
                            new[] { nameof(MarcaOficial) });
                    }
                    break;
            }
        }
    }
}