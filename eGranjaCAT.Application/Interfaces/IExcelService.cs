using eGranjaCAT.Application.ExportMappings;


namespace eGranjaCAT.Infrastructure.Services
{
    public interface IExcelService
    {
        Task<MemoryStream> GenerateExcelAsync<T>(IEnumerable<T> data, List<ExcelColumnMap<T>> maps, string sheetName);
    }
}