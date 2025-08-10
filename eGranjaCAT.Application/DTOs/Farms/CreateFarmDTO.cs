using System.ComponentModel.DataAnnotations;


namespace eGranjaCAT.Application.DTOs.Farm
{
    public class CreateFarmDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Address { get; set; }
        [Required]
        public required string CodiREGA { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
    }
}