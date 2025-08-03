using eGranjaCAT.Application.Entities;
using eGranjaCAT.Domain.Common;

namespace eGranjaCAT.Domain.Entities
{
    public class Lot
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool Active { get; set; } = true;

        public int FarmId { get; set; }
        public Farm Farm { get; set; } = null!;

        public string UserGuid { get; set; }
        public IUserBase User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}