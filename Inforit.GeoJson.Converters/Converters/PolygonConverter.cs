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
    /// Used to serialize and deserialize Polygon objects
    /// </summary>
    public class PolygonConverter : JsonConverter<Polygon>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Polygon);
        }

        /// <inheritdoc />
        public override Polygon Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var polygonPositions = new List<List<Position>>();

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

                var polygonPosition = new List<Position>();

                while (coordinate.MoveNext())
                {
                    var currentPositions = coordinate.Current.EnumerateArray();

                    polygonPosition.Add(new Position(currentPositions.First().GetDouble(), currentPositions.Last().GetDouble()));
                }

                polygonPositions.Add(polygonPosition);
            }

            // Take this array of arrays of arrays and create linear rings
            // and use those to create create polygons
            return new Polygon(polygonPositions.Select(x => new LinearRing(x)));
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Polygon value, JsonSerializerOptions options)
        {
            var polygon = value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(polygon.Type));
            writer.WriteStringValue(polygon.Type.ToString());

            writer.WriteStartArray(nameof(polygon.Coordinates));

            foreach (var linearRing in polygon.Coordinates)
            {
                writer.WriteStartArray();

                foreach (var position in linearRing.Coordinates)
                {
                    writer.WritePosition(position);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}