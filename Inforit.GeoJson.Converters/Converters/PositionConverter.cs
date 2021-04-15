using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using BAMCIS.GeoJSON;

namespace Inforit.GeoJson.Converters
{
    /// <summary>
    /// Used to serialize and deserialize Position objects
    /// </summary>
    public class PositionConverter : JsonConverter<Position>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Position);
        }

        /// <inheritdoc />
        public override Position Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the JSON document
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonElement = jsonDocument.RootElement;

            // GetDouble() could potentially lead to errors,
            // see https://stackoverflow.com/questions/61316308/system-text-json-serializes-a-value-1-0-of-type-double-to-an-value-1-of-type-int
            // see https://github.com/dotnet/runtime/issues/35195

            var longitude = jsonElement[0].GetDouble();
            var latitude = jsonElement[1].GetDouble();
            var elevation = double.NaN;

            if (jsonElement.GetArrayLength() == 3)
            {
                elevation = jsonElement[2].GetDouble();
            }

            return new Position(longitude, latitude, elevation);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Position value, JsonSerializerOptions options)
        {
            var position = value;

            writer.WriteStartArray();

            if (position.HasElevation())
            {
                writer.WriteNumberValue(position.Longitude);
                writer.WriteNumberValue(position.Latitude);
                writer.WriteNumberValue(position.Elevation);
            }
            else
            {
                writer.WriteNumberValue(position.Longitude);
                writer.WriteNumberValue(position.Latitude);
            }
            writer.WriteEndArray();
        }
    }
}