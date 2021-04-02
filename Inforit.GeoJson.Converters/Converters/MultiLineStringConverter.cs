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
    /// Used to serialize and deserialize MultiLineString objects
    /// </summary>
    public class MultiLineStringConverter : JsonConverter<MultiLineString>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MultiLineString);
        }

        /// <inheritdoc />
        public override MultiLineString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

            return new MultiLineString(polygonPositions.Select(x => new LineString(x)));
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, MultiLineString value, JsonSerializerOptions options)
        {
            var mls = value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(mls.Type));
            writer.WriteStringValue(mls.Type.ToString());

            writer.WriteStartArray(nameof(mls.Coordinates));

            foreach (var linearRing in mls.Coordinates)
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