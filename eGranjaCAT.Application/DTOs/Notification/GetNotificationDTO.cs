using eGranjaCAT.Application.DTOs.User;


namespace eGranjaCAT.Application.DTOs.Notification
{
    public class GetNotificationDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } 
        public bool IsRead { get; set; }
        public DateTime ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Pending { get; set; }
        public GetUserDTO User { get; set; }
    }
}
