using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using BAMCIS.GeoJSON;
using Inforit.GeoJson.Converters.Extensions;

namespace Inforit.GeoJson.Converters
{
    /// <summary>
    /// Used to serialize and deserialize LineString objects
    /// </summary>
    public class LineStringConverter : JsonConverter<LineString>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(LineString);
        }

        /// <inheritdoc />
        public override LineString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var polygonPositions = new List<Position>();

            // Read the JSON document
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonElement = jsonDocument.RootElement;

            // JsonElement TryGetProperty or GetProperty methods are case sensitive
            var coordinatesProperty = jsonElement
                                      .EnumerateObject()
                                      .First(p => string.Equals(p.Name, "coordinates", StringComparison.OrdinalIgnoreCase));

            var coordinates = coordinatesProperty.Value.EnumerateArray();

            while (coordinates.MoveNext())
            {
                var coordinate = coordinates.Current.EnumerateArray();

                polygonPositions.Add(new Position(coordinate.First().GetDouble(), coordinate.Last().GetDouble()));
            }

            // Take this array of arrays of arrays and create linear rings
            // and use those to create create polygons
            return new LineString(polygonPositions);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, LineString value, JsonSerializerOptions options)
        {
            var lineString = value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(lineString.Type));
            writer.WriteStringValue(lineString.Type.ToString());

            writer.WriteStartArray(nameof(lineString.Coordinates));

            foreach (var position in lineString.Coordinates)
            {
                writer.WritePosition(position);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}