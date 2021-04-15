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
    /// Used to serialize and deserialize Feature objects
    /// </summary>
    public class FeatureConverter : JsonConverter<Feature>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Feature);
        }

        /// <inheritdoc />
        public override Feature Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

            var featureIdProperty = jsonElement
                                    .EnumerateObject()
                                    .FirstOrDefault(p => p.Name.Contains("id", StringComparison.OrdinalIgnoreCase));

            FeatureId featureId = null;
            if (featureIdProperty.Value.ValueKind != JsonValueKind.Undefined)
            {
                featureId = featureIdProperty.Value.ToObject<FeatureId>(options);
            }

            var boundingBoxProperty = jsonElement
                                      .EnumerateObject()
                                      .FirstOrDefault(p => p.Name.Contains("bbox", StringComparison.OrdinalIgnoreCase));

            IEnumerable<double> boundingBox = null;
            if (boundingBoxProperty.Value.ValueKind != JsonValueKind.Undefined)
            {
                boundingBox = boundingBoxProperty.Value.ToObject<IEnumerable<double>>(options);
            }

            var geometryProperty = jsonElement
                               .EnumerateObject()
                               .FirstOrDefault(p => p.Name.Contains("geometry", StringComparison.OrdinalIgnoreCase));

            Geometry geometry = null;
            if (geometryProperty.Value.ValueKind != JsonValueKind.Undefined)
            {
                geometry = geometryProperty.Value.ToObject<Geometry>(options);
            }

            var propertiesProperty = jsonElement
                               .EnumerateObject()
                               .FirstOrDefault(p => p.Name.Contains("properties", StringComparison.OrdinalIgnoreCase));

            IDictionary<string, dynamic> properties = null;
            if (propertiesProperty.Value.ValueKind != JsonValueKind.Undefined)
            {
                properties = propertiesProperty.Value.ToObject<IDictionary<string, dynamic>>(options);
            }

            return new Feature(geometry, properties, boundingBox, featureId);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Feature value, JsonSerializerOptions options)
        {
            var feature = value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(feature.Type));
            writer.WriteStringValue(feature.Type.ToString());

            var featureId = feature.Id;

            if (featureId is not null)
            {
                writer.WritePropertyName(nameof(feature.Id));
                JsonSerializer.Serialize(writer, feature.Id, options);
            }

            var boundingBox = feature.BoundingBox;

            if (boundingBox is not null)
            {
                writer.WritePropertyName("bbox");
                JsonSerializer.Serialize(writer, boundingBox, options);
            }

            var geometry = feature.Geometry;

            if (geometry is not null)
            {
                writer.WritePropertyName(nameof(feature.Geometry));

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
            else
            {
                writer.WriteNull(nameof(geometry));
            }

            if (feature.Properties.Any())
            {
                writer.WritePropertyName(nameof(feature.Properties));

                writer.WriteStartObject();
                foreach (var property in feature.Properties)
                {
                    writer.WritePropertyName(property.Key);
                    JsonSerializer.Serialize(writer, property.Value);
                }
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}