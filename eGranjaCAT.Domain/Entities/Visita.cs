using eGranjaCAT.Application.Entities;

namespace eGranjaCAT.Domain.Entities
{
    public class Visita
    {
        public int Id { get; set; }
        public required string Visitant { get; set; }
        public required DateTime Data { get; set; }
        public required string Motiu { get; set; }

        public string? Matricula { get; set; }
        public string? Empresa { get; set; }
        public string? DarreraExplotacio { get; set; }
        public DateTime? DataDarreraExplotacio { get; set; }
        public string? Observacions { get; set; }

        public string UserGuid { get; set; } = null!;

        public int FarmId { get; set; }
        public Farm Farm { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Updated { get; set; } = false;
    }
}