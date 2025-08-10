namespace eGranjaCAT.Application.DTOs.Visites
{
    public class UpdateVisitaDTO
    {
        public string Visitant { get; set; }
        public DateTime Data { get; set; }
        public string Motiu { get; set; }

        public string Matricula { get; set; }
        public string Empresa { get; set; }
        public string DarreraExplotacio { get; set; }
        public DateTime DataDarreraExplotacio { get; set; }
        public string Observacions { get; set; }
    }
}
