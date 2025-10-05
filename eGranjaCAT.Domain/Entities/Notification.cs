using eGranjaCAT.Domain.Enums;


namespace eGranjaCAT.Domain.Entities
{
    public class Notification
    {
        public string Id { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationTypesEnum Type { get; set; }
        public NotificationPriorityEnum Priority { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserGuid { get; set; } = string.Empty;
        public bool? Pending { get; set; }
    }
}