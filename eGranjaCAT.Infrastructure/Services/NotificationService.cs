using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Entrada;
using eGranjaCAT.Application.DTOs.Notification;
using eGranjaCAT.Infrastructure.Data;
using eGranjaCAT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace eGranjaCAT.Infrastructure.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly Mapper _mapper;
        private readonly UserManager<User> _userManager;

        public NotificationService(ApplicationDbContext context, Mapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task SendNotificationAsync(string userId, string message)
        {
            await _context.SaveChangesAsync();
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
    }
}
