using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using BAMCIS.GeoJSON;

using Inforit.GeoJson.Converters.Extensions;

namespace Inforit.GeoJson.Converters
{
    /// <summary>
    /// Used to serialize and deserialize Geometry objects
    /// </summary>
    public class GeometryConverter : JsonConverter<object>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Geometry);
        }

        /// <inheritdoc />
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Geometry can be null in a feature
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            // Read the `className` from our JSON document
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonElement = jsonDocument.RootElement;

            var typeProperty = jsonElement
                              .EnumerateObject()
                              .FirstOrDefault(p => p.Name.Contains("type", StringComparison.OrdinalIgnoreCase));

            if (typeProperty.Value.ValueKind == JsonValueKind.Undefined)
            {
                throw new JsonException("Invalid geojson geometry object, does not have 'type' field.");
            }

            var geoJsonType = typeProperty.Value.ToObject<GeoJsonType>(options);
            var actualType = BAMCIS.GeoJSON.GeoJson.GetType(geoJsonType);

            if (typeToConvert is null || typeToConvert != actualType)
            {
                return (BAMCIS.GeoJSON.GeoJson)jsonDocument.ToObject(actualType, options);
            }
            else
            {
                // In the original GeoJSON converters project an object populate was performed
                // on the serializer. No case found yet to have this in here as well
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            var geometry = (Geometry)value;

            switch (geometry.Type)
            {
                case GeoJsonType.Point:
                    {
                        JsonSerializer.Serialize(writer, (Point)geometry, options);
                        break;
                    }
                case GeoJsonType.LineString:
                    {
                        JsonSerializer.Serialize(writer, (LineString)geometry, options);
                        break;
                    }
                case GeoJsonType.Polygon:
                    {
                        JsonSerializer.Serialize(writer, (Polygon)geometry, options);
                        break;
                    }
                case GeoJsonType.MultiPoint:
                    {
                        JsonSerializer.Serialize(writer, (MultiPoint)geometry, options);
                        break;
                    }
                case GeoJsonType.MultiLineString:
                    {
                        JsonSerializer.Serialize(writer, (MultiLineString)geometry, options);
                        break;
                    }
                case GeoJsonType.MultiPolygon:
                    {
                        JsonSerializer.Serialize(writer, (MultiPolygon)geometry, options);
                        break;
                    }
            }
        }
    }
}