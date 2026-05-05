using System.Text.Json.Serialization;
using Common.JsonConverters;

namespace RoyalVilla.Api.Dto;

/// <summary>
/// Create Villa Post Body Data
/// </summary>
public class CreateOrUpdateVillaDto
{
    /// <summary>
    /// Name of the villa
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Detail Description of the villa
    /// </summary>
    public string? Details { get; set; }
    
    /// <summary>
    /// Rate of the villa
    /// </summary>
    [JsonConverter(typeof(NullableDecimalConverter))]
    public decimal? Rate { get; set; }
    
    /// <summary>
    /// Square feet of the villa
    /// </summary>
    [JsonConverter(typeof(NullableIntegerConverter))]
    public int? SquareFeet { get; set; }
    
    /// <summary>
    /// Occupancy capacity of the villa
    /// </summary>
    [JsonConverter(typeof(NullableIntegerConverter))]
    public int? Occupancy { get; set; }
    
    /// <summary>
    /// Image URL of the villa
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Stringify
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"Name={Name}, Details={Details}, Rate={Rate}, SquareFeet={SquareFeet}, Occupancy={Occupancy}, ImageUrl={ImageUrl}";
    }
}