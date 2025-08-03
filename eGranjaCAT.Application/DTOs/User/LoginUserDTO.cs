using System.ComponentModel.DataAnnotations;


namespace eGranjaCAT.Application.DTOs.User
{
    public class LoginUserDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}