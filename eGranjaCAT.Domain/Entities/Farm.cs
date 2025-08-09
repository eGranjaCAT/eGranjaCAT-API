namespace eGranjaCAT.Application.Entities
{
    public class Farm
    {
        public int Id { get; set; }
        public required string CodiREGA { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public required string UserGuid { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool Updated { get; set; } = false;
    }
}