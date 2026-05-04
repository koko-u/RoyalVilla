using System;

namespace RoyalVilla.Api.Repositories;

public sealed class VillaRow
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Details { get; set; }
    public decimal Rate { get; set; } = 0.0m;
    public int SquareFeet { get; set; } = 0;
    public int Occupancy { get; set; } = 0;
    public string? ImageUrl { get; set; }
}
