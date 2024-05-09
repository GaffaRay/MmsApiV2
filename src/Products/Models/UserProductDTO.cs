namespace MmsApiV2.Products.Models;

/// <summary>
/// Represents a product bookable for gym members.
/// </summary>
public class UserProductDTO
{
    /// <summary>
    /// The ID of the product.
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// The date from which the specified product is active and can be used by the user.<br/>
    /// It must be today’s or future date in Timezone of the club.<br/>
    /// Format: yyyy-MM-dd
    /// </summary>
    public string StartDate { get; set; }

    /// <summary>
    /// The date to which the specified product is active and can be used by the user.<br/>
    /// It must be future date in Timezone of the club.<br/>
    /// Format: yyyy-MM-dd
    /// </summary>
    public string EndDate { get; set; }
}
