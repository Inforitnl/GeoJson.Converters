using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using BAMCIS.GeoJSON;

using Inforit.GeoJson.Converters.Extensions;

namespace Inforit.GeoJson.Converters
{
    /// <summary>
    /// Used to serialize and deserialize GeoJson objects
    /// </summary>
    public class GeoJsonConverter : JsonConverter<BAMCIS.GeoJSON.GeoJson>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(BAMCIS.GeoJSON.GeoJson);
        }

        /// <inheritdoc />
        public override BAMCIS.GeoJSON.GeoJson Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Allow an abstract GeoJson to be null in deserialization
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

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

            var actualType = BAMCIS.GeoJSON.GeoJson.GetType(typeProperty.Value.ToObject<GeoJsonType>(options));

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
        public override void Write(Utf8JsonWriter writer, BAMCIS.GeoJSON.GeoJson value, JsonSerializerOptions options)
        {
            switch (value.Type)
            {
                case GeoJsonType.Feature:
                    {
                        JsonSerializer.Serialize(writer, (Feature)value, options);
                        break;
                    }
                case GeoJsonType.FeatureCollection:
                    {
                        JsonSerializer.Serialize(writer, (FeatureCollection)value, options);
                        break;
                    }
                case GeoJsonType.GeometryCollection:
                    {
                        JsonSerializer.Serialize(writer, (GeometryCollection)value, options);
                        break;
                    }
                case GeoJsonType.Point:
                    {
                        JsonSerializer.Serialize(writer, (Point)value, options);
                        break;
                    }
                case GeoJsonType.LineString:
                    {
                        JsonSerializer.Serialize(writer, (LineString)value, options);
                        break;
                    }
                case GeoJsonType.Polygon:
                    {
                        JsonSerializer.Serialize(writer, (Polygon)value, options);
                        break;
                    }
                case GeoJsonType.MultiPoint:
                    {
                        JsonSerializer.Serialize(writer, (MultiPoint)value, options);
                        break;
                    }
                case GeoJsonType.MultiLineString:
                    {
                        JsonSerializer.Serialize(writer, (MultiLineString)value, options);
                        break;
                    }
                case GeoJsonType.MultiPolygon:
                    {
                        JsonSerializer.Serialize(writer, (MultiPolygon)value, options);
                        break;
                    }
                default:
                    throw new NotImplementedException("Invalid geojson object");
            }
        }
    }
}