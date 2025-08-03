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
                new ExcelColumnMap<Entrada> { Header = "Creador", ValueSelector = e => e.User?.Email ?? string.Empty },
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
                new ExcelColumnMap<Lot> { Header = "Creador", ValueSelector = e => e.User?.Email ?? string.Empty },
                new ExcelColumnMap<Lot> { Header = "Data de creació", ValueSelector = e => e.CreatedAt.ToString("dd/MM/yyyy HH:mm") },
            };
    }
}