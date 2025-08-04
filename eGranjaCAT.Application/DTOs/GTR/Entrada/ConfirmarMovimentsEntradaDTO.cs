using System.ComponentModel.DataAnnotations;

namespace eGranjaCAT.Application.DTOs.GTR.Entrada
{
    public class ConfirmarMovimentsEntradaDTO
    {
        [Required, Length(9, 9)]
        public required string Nif { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required, Length(6, 6)]
        public required string MarcaOficialDesti { get; set; }
        [Required, Length(17, 17)]
        public required string CodiRemo { get; set; }
        [Required, Length(9, 9)]
        public required string NifConductor { get; set; }
        [Required, Length(30, 30)]
        public required string Matricula { get; set; }
        [Required]
        public required int NombreAnimals { get; set; }
    }
}