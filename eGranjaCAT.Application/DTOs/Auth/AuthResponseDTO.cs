namespace eGranjaCAT.Application.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public required string Token { get; set; }
        public DateTime Expiraton { get; set; }
    }
}