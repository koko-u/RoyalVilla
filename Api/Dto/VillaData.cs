namespace RoyalVilla.Api.Dto;

/// <summary>
/// Villa Data
/// </summary>
public class VillaData
{
    /// <summary>
    /// Identifier of the villa
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Name of the villa
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Detailed description of the villa
    /// </summary>
    public string? Details { get; set; }
    
    /// <summary>
    /// Rate of the villa
    /// </summary>
    public decimal Rate { get; set; } = 0.0m;
    
    /// <summary>
    /// Square footage of the villa
    /// </summary>
    public int SquareFeet { get; set; } = 0;
    
    /// <summary>
    /// Occupancy capacity of the villa
    /// </summary>
    public int Occupancy { get; set; } = 0;
    
    /// <summary>
    /// Image URL of the villa
    /// </summary>
    public string? ImageUrl { get; set; }
}