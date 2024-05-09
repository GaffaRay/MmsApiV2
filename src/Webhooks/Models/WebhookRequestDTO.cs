namespace MmsApiV2.Webhooks.Models;

/// <summary>
/// Data Transfer Object for Webhook Requests.
/// </summary>
public class WebhookRequestDTO
{
    /// <summary>
    /// Webhook URL to send the event to.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Secret to sign the payload with.
    /// </summary>
    public string Secret { get; set; }
}
