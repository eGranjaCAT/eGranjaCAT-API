using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Notification;
using eGranjaCAT.Domain.Enums;

namespace eGranjaCAT.Infrastructure.Services
{
    public interface INotificationService
    {
        Task<ServiceResult<List<GetNotificationDTO>>> GetNotificationsAsync(string userId);
        Task<ServiceResult<List<GetNotificationDTO>>> GetNotificationsByPriorityAsync(string userId, NotificationPriorityEnum notificationPriority);
        Task<ServiceResult<bool>> MarkAsReadAsync(string notificationId);
    }
}