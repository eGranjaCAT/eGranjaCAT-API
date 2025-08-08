namespace eGranjaCAT.Application.Entities
{
    public class Farm
    {
        public int Id { get; set; }
        public required string CodiREGA { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}