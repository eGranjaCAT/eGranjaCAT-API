using eGranjaCAT.Domain.Enums;


namespace eGranjaCAT.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public NotificationTypesEnum Type { get; set; }
        public NotificationPriorityEnum Priority { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime ReadAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserGuid { get; set; } = string.Empty;
        public bool? Pending { get; set; }
    }
}