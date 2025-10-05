using System.ComponentModel.DataAnnotations;


namespace eGranjaCAT.Application.DTOs.Visites
{
    public class CreateVisitaDTO
    {
        [Required]
        public required string Visitant { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public required DateTime Data { get; set; }
        [Required]
        public required string Motiu { get; set; }

        public string? Matricula { get; set; }
        public string? Empresa { get; set; }
        public string? DarreraExplotacio { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DataDarreraExplotacio { get; set; }
        public string? Observacions { get; set; }
    }
}