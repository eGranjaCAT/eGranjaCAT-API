using eGranjaCAT.Application.DTOs.Farm;
using eGranjaCAT.Application.DTOs.Lot;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Domain.Enums;
using System.Text.Json.Serialization;


namespace eGranjaCAT.Application.DTOs.Entrada
{
    public class GetEntradaDTO
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CategoriesEnum Categoria { get; set; }

        public DateTime Data { get; set; }

        public int NombreAnimals { get; set; }

        public double PesTotal { get; set; }

        public double PesIndividual { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrigensEnum Origen { get; set; }

        public string? MarcaOficial { get; set; }

        public string? CodiREGA { get; set; }

        public string NumeroDocumentTrasllat { get; set; } = null!;

        public string? Observacions { get; set; }

        public GetUserDTO User { get; set; }
        public GetLotNoRelationsDTO Lot { get; set; }
        public GetFarmDTO Farm { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Updated { get; set; }
    }
}