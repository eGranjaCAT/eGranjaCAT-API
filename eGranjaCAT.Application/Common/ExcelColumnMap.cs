namespace eGranjaCAT.Application.ExportMappings
{
    public class ExcelColumnMap<T>
    {
        public string Header { get; set; } = string.Empty;
        public Func<T, object?> ValueSelector { get; set; } = _ => null!;

        public Action<T, string>? ValueSetter { get; set; }
    }
}