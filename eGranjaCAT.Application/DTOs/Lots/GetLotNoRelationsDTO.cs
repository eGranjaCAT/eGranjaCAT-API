namespace eGranjaCAT.Application.DTOs.Lot
{
    public class GetLotNoRelationsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}