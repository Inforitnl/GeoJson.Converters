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
    /// Used to serialize and deserialize MultiPoint objects
    /// </summary>
    public class MultiPointConverter : JsonConverter<MultiPoint>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MultiPoint);
        }

        /// <inheritdoc />
        public override MultiPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var polygonPositions = new List<Position>();

            // Read the JSON document
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonElement = jsonDocument.RootElement;

            // JsonElement TryGetProperty or GetProperty methods are case sensitive
            var coordinatesProperty = jsonElement
                                      .EnumerateObject()
                                      .First(p => string.Compare(p.Name, "coordinates", StringComparison.OrdinalIgnoreCase) == 0);

            var coordinates = coordinatesProperty.Value.EnumerateArray();

            while (coordinates.MoveNext())
            {
                var coordinate = coordinates.Current.EnumerateArray();

                polygonPositions.Add(new Position(coordinate.First().GetDouble(), coordinate.Last().GetDouble()));
            }

            return new MultiPoint(polygonPositions);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, MultiPoint value, JsonSerializerOptions options)
        {
            var multiPoint = value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(multiPoint.Type));
            writer.WriteStringValue(multiPoint.Type.ToString());

            writer.WriteStartArray(nameof(multiPoint.Coordinates));

            foreach (var position in multiPoint.Coordinates)
            {
                writer.WritePosition(position);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}