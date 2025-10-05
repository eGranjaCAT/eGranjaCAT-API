using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Notification;
using eGranjaCAT.Domain.Enums;
using eGranjaCAT.Infrastructure.Data;
using eGranjaCAT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace eGranjaCAT.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public NotificationService(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<ServiceResult<List<GetNotificationDTO>>> GetNotificationsAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return ServiceResult<List<GetNotificationDTO>>.Fail("User not found");

                var notifications = await _context.Notifications.Where(n => n.UserGuid == userId).OrderByDescending(n => n.CreatedAt).ToListAsync();
                var notificationDTOs = _mapper.Map<List<GetNotificationDTO>>(notifications);

                return ServiceResult<List<GetNotificationDTO>>.Ok(notificationDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<GetNotificationDTO>>.Fail($"Error retrieving notifications: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<GetNotificationDTO>>> GetNotificationsByPriorityAsync(string userId, NotificationPriorityEnum notificationPriority)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return ServiceResult<List<GetNotificationDTO>>.Fail("User not found");

                var notifications = await _context.Notifications.Where(n => n.UserGuid == userId).Where(n => n.Priority == notificationPriority).OrderByDescending(n => n.CreatedAt).ToListAsync();
                var notificationDTOs = _mapper.Map<List<GetNotificationDTO>>(notifications);

                return ServiceResult<List<GetNotificationDTO>>.Ok(notificationDTOs);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<GetNotificationDTO>>.Fail($"Error retrieving notifications: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> MarkAsReadAsync(string notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
                if (notification == null) return ServiceResult<bool>.Fail("Notification not found");

                notification.IsRead = true;
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Fail($"Error marking notification as read: {ex.Message}");
            }
        }
    }
}
