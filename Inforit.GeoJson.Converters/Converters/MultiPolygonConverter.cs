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
    /// Used to serialize and deserialize MultiPolygon objects
    /// </summary>
    public class MultiPolygonConverter : JsonConverter<MultiPolygon>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MultiPolygon);
        }

        /// <inheritdoc />
        public override MultiPolygon Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var polygonPositions = new List<List<List<Position>>>();

            // Read the JSON document
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonElement = jsonDocument.RootElement;

            // JsonElement TryGetProperty or GetProperty methods are case sensitive
            var coordinatesProperty = jsonElement
                                      .EnumerateObject()
                                      .First(p => string.Equals(p.Name, "coordinates", StringComparison.OrdinalIgnoreCase));

            var coordinatesRoot = coordinatesProperty.Value.EnumerateArray();

            while (coordinatesRoot.MoveNext())
            {
                var coordinatesArray = coordinatesRoot.Current.EnumerateArray();
                var multiPolygonContainer = new List<List<Position>>();

                while (coordinatesArray.MoveNext())
                {
                    var polygonArray = coordinatesArray.Current.EnumerateArray();
                    var polygonContainer = new List<Position>();

                    while (polygonArray.MoveNext())
                    {
                        var coordinates = polygonArray.Current.EnumerateArray();

                        polygonContainer.Add(new Position(coordinates.First().GetDouble(), coordinates.Last().GetDouble()));
                    }
                    multiPolygonContainer.Add(polygonContainer);
                }
                polygonPositions.Add(multiPolygonContainer);
            }

            // Take this array of arrays of arrays and create linear rings
            // and use those to create create polygons
            return new MultiPolygon(polygonPositions.Select(x => new Polygon(x.Select(y => new LinearRing(y)))));
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, MultiPolygon value, JsonSerializerOptions options)
        {
            var mp = value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(mp.Type));
            writer.WriteStringValue(mp.Type.ToString());

            writer.WriteStartArray(nameof(mp.Coordinates));

            foreach (var polygon in mp.Coordinates)
            {
                writer.WriteStartArray();
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
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}