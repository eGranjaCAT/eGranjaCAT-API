using eGranjaCAT.Application.Entities;


namespace eGranjaCAT.Domain.Entities
{
    public class NetejaDDD
    {
        public required int Id { get; set; }
        public required DateTime Data { get; set; }
        public required string Descripcio { get; set; }
        public required string AplicadorGuid { get; set; }
        public required string Producte { get; set; }
        public required string LlocAplicacio { get; set; }
        public string? Observacions { get; set; }
        public required string UserGuid { get; set; }
        public required int FarmId { get; set; }
        public required Farm Farm { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool Updated { get; set; } = false;
    }
}