namespace MmsApiV2.Products.Models;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class ProductDTO
{
    /// <summary>
    /// The ID of the product.
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Product title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Product description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The scope of the bookable package.<br/>
    /// Allowed values: LOCATION, CHAIN, BRAND
    /// </summary>
    public string Scope { get; set; }
}
