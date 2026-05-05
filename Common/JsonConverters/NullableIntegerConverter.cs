using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.JsonConverters;

/// <summary>
/// Convert string to decimal? value which allows null and empty string
/// </summary>
public class NullableIntegerConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            var intStyle =
                NumberStyles.Integer |
                NumberStyles.AllowLeadingSign |
                NumberStyles.AllowLeadingWhite |
                NumberStyles.AllowTrailingWhite |
                NumberStyles.AllowThousands;
            
            if (int.TryParse(s, intStyle, CultureInfo.InvariantCulture , out var result))
            {
                return result;
            }
        }

        throw new JsonException("The value must be int value");
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteNumberValue(value.Value);
        }
    }
}