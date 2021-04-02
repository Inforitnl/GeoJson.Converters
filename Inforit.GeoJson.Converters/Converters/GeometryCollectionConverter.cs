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
    /// Provides conversion of a GeometryCollection object
    /// </summary>
    public class GeometryCollectionConverter : JsonConverter<GeometryCollection>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(GeometryCollection);
        }

        /// <inheritdoc />
        public override GeometryCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var geometryCollection = new List<Geometry>();

            // Read the JSON document
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonElement = jsonDocument.RootElement;

            var typeProperty = jsonElement
                                   .EnumerateObject()
                                   .FirstOrDefault(p => p.Name.Contains("type", StringComparison.OrdinalIgnoreCase));

            if (typeProperty.Value.ValueKind == JsonValueKind.Undefined)
            {
                throw new JsonException("Invalid geojson object, does not have 'type' field.");
            }

            var geometriesProperty = jsonElement
                                         .EnumerateObject()
                                         .FirstOrDefault(p => p.Name.Contains("geometries", StringComparison.OrdinalIgnoreCase));

            if (geometriesProperty.Value.ValueKind == JsonValueKind.Undefined)
            {
                throw new JsonException("Invalid geojson object, does not have 'geometries' field.");
            }

            var geometries = geometriesProperty.Value.EnumerateArray();

            while (geometries.MoveNext())
            {
                var geometry = geometries.Current;

                geometryCollection.Add(geometry.ToObject<Geometry>(options));
            }

            return new GeometryCollection(geometryCollection);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, GeometryCollection value, JsonSerializerOptions options)
        {
            var geometryCollection = value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(geometryCollection.Type));
            writer.WriteStringValue(geometryCollection.Type.ToString());

            writer.WritePropertyName(nameof(geometryCollection.Geometries));
            writer.WriteStartArray();

            foreach (var geometry in geometryCollection.Geometries)
            {
                JsonSerializer.Serialize(writer, geometry, options);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}