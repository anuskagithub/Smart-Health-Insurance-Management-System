namespace HealthInsuranceApi.DTOs.Notification
{
    public class NotificationReadDto
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsRead { get; set; }
    }
}
