using System;

namespace RoyalVilla.Api.Repositories;

/// <summary>
/// Dapper Query Result Row for Villa
/// </summary>
public sealed class VillaRow
{
    /// <summary>
    /// Id of the Villa
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the Villa
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Detail Description of the Villa
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Rate of the Villa per night
    /// </summary>
    public decimal Rate { get; set; } = 0.0m;

    /// <summary>
    /// Square footage of the Villa
    /// </summary>
    public int SquareFeet { get; set; } = 0;

    /// <summary>
    /// Occupancy capacity of the Villa
    /// </summary>
    public int Occupancy { get; set; } = 0;

    /// <summary>
    /// Image URL of the Villa
    /// </summary>
    public string? ImageUrl { get; set; }
}
