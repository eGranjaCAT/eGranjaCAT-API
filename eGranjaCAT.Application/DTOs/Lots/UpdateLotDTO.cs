namespace eGranjaCAT.Application.DTOs.Lot
{
    public class UpdateLotDTO
    {
        public string? Name { get; set; }
        public bool? Active { get; set; } = true;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}