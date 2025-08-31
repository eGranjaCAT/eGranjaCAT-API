using eGranjaCAT.Application.ExportMappings;
using eGranjaCAT.Domain.Entities;


namespace eGranjaCAT.Infrastructure.ExportMappings
{
    public static class ExcelColumnMappings
    {
        public static readonly List<ExcelColumnMap<Entrada>> EntradaExcelColumnMappings =
            new()
            {
                new ExcelColumnMap<Entrada> { Header = "ID", ValueSelector = e => e.Id },
                new ExcelColumnMap<Entrada> { Header = "Explotació", ValueSelector = e => e.Farm?.Name ?? string.Empty },
                new ExcelColumnMap<Entrada> { Header = "Data", ValueSelector = e => e.Data.ToString("dd/MM/yyyy") },
                new ExcelColumnMap<Entrada> { Header = "Nombre d'animals", ValueSelector = e => e.NombreAnimals },
                new ExcelColumnMap<Entrada> { Header = "Pes total", ValueSelector = e => e.PesTotal },
                new ExcelColumnMap<Entrada> { Header = "Pes individual", ValueSelector = e => e.PesIndividual },
                new ExcelColumnMap<Entrada> { Header = "Lot", ValueSelector = e => e.Lot?.Name ?? string.Empty },
                new ExcelColumnMap<Entrada> { Header = "Origen", ValueSelector = e => e.Origen },
                new ExcelColumnMap<Entrada> { Header = "Marca oficial", ValueSelector = e => e.MarcaOficial },
                new ExcelColumnMap<Entrada> { Header = "Codi REGA", ValueSelector = e => e.CodiREGA },
                new ExcelColumnMap<Entrada> { Header = "Número de document de trasllat", ValueSelector = e => e.NumeroDocumentTrasllat },
                new ExcelColumnMap<Entrada> { Header = "Data de creació", ValueSelector = e => e.CreatedAt.ToString("dd/MM/yyyy HH:mm") },
                new ExcelColumnMap<Entrada> { Header = "Observacions", ValueSelector = e => e.Observacions }
            };


        public static readonly List<ExcelColumnMap<Lot>> LotExcelColumnMappings =
            new()
            {
                new ExcelColumnMap<Lot> { Header = "ID", ValueSelector = e => e.Id },
                new ExcelColumnMap<Lot> { Header = "Explotació", ValueSelector = e => e.Farm?.Name ?? string.Empty },
                new ExcelColumnMap<Lot> { Header = "Nom", ValueSelector = e => e.Name ?? string.Empty },
                new ExcelColumnMap<Lot> { Header = "Actiu", ValueSelector = e => e.Active },
                new ExcelColumnMap<Lot> { Header = "Data de creació", ValueSelector = e => e.CreatedAt.ToString("dd/MM/yyyy HH:mm") },
            };

        public static readonly List<ExcelColumnMap<Visita>> VisitaExcelColumnMappings =
            new()
            {
                new ExcelColumnMap<Visita> { Header = "ID", ValueSelector = v => v.Id },
                new ExcelColumnMap<Visita> { Header = "Explotació", ValueSelector = v => v.Farm?.Name ?? string.Empty },
                new ExcelColumnMap<Visita> { Header = "Visitant", ValueSelector = v => v.Visitant },
                new ExcelColumnMap<Visita> { Header = "Data", ValueSelector = v => v.Data.ToString("dd/MM/yyyy") },
                new ExcelColumnMap<Visita> { Header = "Motiu", ValueSelector = v => v.Motiu },
                new ExcelColumnMap<Visita> { Header = "Matricula", ValueSelector = v => v.Matricula },
                new ExcelColumnMap<Visita> { Header = "Empresa", ValueSelector = v => v.Empresa },
                new ExcelColumnMap<Visita> { Header = "Darrera Explotació", ValueSelector = v => v.DarreraExplotacio },
                new ExcelColumnMap<Visita> { Header = "Data Darrera Explotació", ValueSelector = v => v.DataDarreraExplotacio?.ToString("dd/MM/yyyy") },
                new ExcelColumnMap<Visita> { Header = "Data de creació", ValueSelector = v => v.CreatedAt.ToString("dd/MM/yyyy HH:mm") },
                new ExcelColumnMap<Visita> { Header = "Observacions", ValueSelector = v => v.Observacions },
            };
    }
}