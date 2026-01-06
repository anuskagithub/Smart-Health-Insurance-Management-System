using HealthInsuranceApi.DTOs.Notification;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(
            string userId,
            string title,
            string message);

        Task<IEnumerable<NotificationReadDto>>
            GetUserNotificationsAsync(string userId);

        Task MarkAsReadAsync(int notificationId);
    }
}
