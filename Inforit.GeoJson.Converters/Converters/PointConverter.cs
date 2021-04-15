using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using BAMCIS.GeoJSON;

namespace Inforit.GeoJson.Converters
{
    /// <summary>
    /// Used to serialize and deserialize Point objects
    /// </summary>
    public class PointConverter : JsonConverter<Point>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Point);
        }

        /// <inheritdoc />
        public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the JSON document
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonElement = jsonDocument.RootElement;

            // JsonElement TryGetProperty or GetProperty methods are case sensitive
            var coordinatesProperty = jsonElement
                                      .EnumerateObject()
                                      .First(p => string.Equals(p.Name, "coordinates", StringComparison.OrdinalIgnoreCase));

            var coordinates = coordinatesProperty.Value;

            var longitude = coordinates[0].GetDouble();
            var latitude = coordinates[1].GetDouble();

            return new Point(new Position(longitude, latitude));
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
        {
            var point = value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(point.Type));
            writer.WriteStringValue(point.Type.ToString());

            writer.WriteStartArray(nameof(point.Coordinates));
            writer.WriteNumberValue(point.Coordinates.Longitude);
            writer.WriteNumberValue(point.Coordinates.Latitude);
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}