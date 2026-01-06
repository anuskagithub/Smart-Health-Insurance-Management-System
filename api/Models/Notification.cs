using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceApi.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
