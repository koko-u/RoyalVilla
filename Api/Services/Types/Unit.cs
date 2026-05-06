using System;
using System.Diagnostics.CodeAnalysis;

namespace RoyalVilla.Api.Services.Types;

/// <summary>
/// Unit , Void type
/// </summary>
public sealed class Unit : IEquatable<Unit>
{
    /// <summary>
    /// Unit value
    /// </summary>
    public static readonly Unit Value = new();

    /// <summary>
    /// always equals unit values
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Unit? other) => other is not null;

    /// <summary>
    /// String representation of unit value
    /// </summary>
    /// <returns></returns>
    public override string ToString() => "()";

    /// <summary>
    /// override equality method
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Unit;

    /// <summary>
    /// Hash code for unit value
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => 0;
}
