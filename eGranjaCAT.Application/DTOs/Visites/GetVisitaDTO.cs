using eGranjaCAT.Application.DTOs.Farm;
using eGranjaCAT.Application.DTOs.User;


namespace eGranjaCAT.Application.DTOs.Visites
{
    public class GetVisitaDTO
    {
        public int Id { get; set; }
        public required string Visitant { get; set; }
        public required DateTime Data { get; set; }
        public required string Motiu { get; set; }

        public string? Matricula { get; set; }
        public string? Empresa { get; set; }
        public string? DarreraExplotacio { get; set; }
        public DateTime? DataDarreraExplotacio { get; set; }
        public string? Observacions { get; set; }

        public GetUserDTO User { get; set; } = null!;
        public GetFarmDTO Farm { get; set; } = null!;

        public bool Updated { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}