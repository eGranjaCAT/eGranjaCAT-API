using Microsoft.AspNetCore.Identity;


namespace eGranjaCAT.Infrastructure.Identity
{
    public class User : IdentityUser
    {
        public required string Name { get; set; }
        public required string Lastname { get; set; }
        public required string Role { get; set; }
        public override string? Email { get; set; }
    }
}