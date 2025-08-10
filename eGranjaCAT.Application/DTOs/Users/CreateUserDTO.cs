using eGranjaCAT.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace eGranjaCAT.Application.DTOs.User
{
    public class CreateUserDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Lastname { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        [EnumDataType(typeof(RolesEnum))]
        public required string Role { get; set; }
        [Required]
        public required List<AccessesEnum> Permissions { get; set; }
    }
}