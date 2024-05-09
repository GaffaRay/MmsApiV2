namespace MmsApiV2.Webhooks.Models;

/// <summary>
/// Data Transfer Object for the response of a webhook.
/// </summary>
public class WebhookResponseDTO : WebhookRequestDTO
{
    /// <summary>
    /// Unique identifier for the webhook.
    /// </summary>
    public long Id { get; set; }
}
