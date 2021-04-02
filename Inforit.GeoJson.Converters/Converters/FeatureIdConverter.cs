using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using BAMCIS.GeoJSON;

namespace Inforit.GeoJson.Converters
{
    /// <summary>
    /// Provides conversion of a FeatureId object to either a string or integer as the value
    /// for a feature's "id" property
    /// </summary>
    public class FeatureIdConverter : JsonConverter<FeatureId>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(FeatureId);
        }

        /// <inheritdoc />
        public override FeatureId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    {
                        return new FeatureId(reader.GetString());
                    }
                case JsonTokenType.Number:
                    {
                        return new FeatureId(long.Parse(reader.GetDouble().ToString()));
                    }
                case JsonTokenType.Null:
                    {
                        return null;
                    }
                default:
                    {
                        throw new FormatException("The feature id was provided in an unexpected format.");
                    }
            }
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, FeatureId value, JsonSerializerOptions options)
        {
            var featureId = value;

            if (featureId.GetOriginalType() == typeof(string))
            {
                writer.WriteStringValue(featureId.Value);
            }
            else
            {
                writer.WriteNumberValue(long.Parse(featureId.Value));
            }
        }
    }
}