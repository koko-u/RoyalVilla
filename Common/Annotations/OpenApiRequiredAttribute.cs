using System;

namespace Common.Annotations;

/// <summary>
/// Indicate required property for OpenAPI documentation
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class OpenApiRequiredAttribute : Attribute;
