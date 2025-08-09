using eGranjaCAT.Application.Entities;


namespace eGranjaCAT.Domain.Entities
{
    public class Lot
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Active { get; set; } = true;

        public int FarmId { get; set; }
        public Farm Farm { get; set; } = null!;

        public required string UserGuid { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool Updated { get; set; } = false;
    }
}