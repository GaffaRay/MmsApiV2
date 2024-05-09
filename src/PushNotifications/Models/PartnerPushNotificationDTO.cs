namespace MmsApiV2.PushNotifications.Models;

/// <summary>
/// Data Transfer Object for Partner Push Notifications.
/// </summary>
public class PartnerPushNotificationDTO
{
    /// <summary>
    /// Notification plain text.<br/>
    /// 0 - 200 characters<br/>
    /// Example: Welcome to the gym!
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Name of the feature for navigate to.<br/>
    /// Example: workouts
    /// </summary>
    public string? Deeplink { get; set; }
}
