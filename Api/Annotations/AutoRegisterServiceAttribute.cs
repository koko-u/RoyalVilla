using System;
using JetBrains.Annotations;

namespace RoyalVilla.Api.Annotations;

/// <summary>
/// Indicates to the IDE that the decorated class is instantiated through dependency injection.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[MeansImplicitUse(
    ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
    ImplicitUseTargetFlags.Itself
)]
public class AutoRegisterServiceAttribute : Attribute;
