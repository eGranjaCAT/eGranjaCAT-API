using System.ComponentModel.DataAnnotations;


namespace eGranjaCAT.Application.DTOs.Lot
{
    public class CreateLotDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}
