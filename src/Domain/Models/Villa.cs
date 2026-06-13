using System;

namespace RoyalVilla.Domain.Models;

public sealed class Villa
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Details { get; set; }

    public decimal? Rate { get; set; }

    public int? SquareFeet { get; set; }

    public int? Occupancy { get; set; }

    public string? ImageUrl { get; set; }
}
