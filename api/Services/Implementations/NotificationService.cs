using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Notification;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceApi.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(
            string userId,
            string title,
            string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationReadDto>>
            GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedOn)
                .Select(n => new NotificationReadDto
                {
                    NotificationId = n.NotificationId,
                    Title = n.Title,
                    Message = n.Message,
                    CreatedOn = n.CreatedOn,
                    IsRead = n.IsRead
                })
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FindAsync(notificationId)
                ?? throw new Exception("Notification not found");

            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}
