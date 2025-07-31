using System.ComponentModel.DataAnnotations;


namespace eGranjaCAT.Application.Entities
{
    public class Farm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Address { get; set; }
        [Required, Phone]
        public required string Phone { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
