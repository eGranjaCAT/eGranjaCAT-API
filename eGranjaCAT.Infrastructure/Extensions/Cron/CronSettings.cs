namespace eGranjaCAT.Infrastructure.Extensions.Cron
{
    public class CronSettings<T>
    {
        public string CronExpression { get; set; } = default!;
        public TimeZoneInfo TimeZone { get; set; } = default!;
    }
}
