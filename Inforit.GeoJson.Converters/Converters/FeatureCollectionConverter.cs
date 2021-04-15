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
    /// Used to serialize and deserialize FeatureCollection object
    /// </summary>
    public class FeatureCollectionConverter : JsonConverter<FeatureCollection>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(FeatureCollection);
        }

        /// <inheritdoc />
        public override FeatureCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var featureCollection = new List<Feature>();

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

            var featuresProperty = jsonElement
                                   .EnumerateObject()
                                   .FirstOrDefault(p => p.Name.Contains("features", StringComparison.OrdinalIgnoreCase));

            if (featuresProperty.Value.ValueKind == JsonValueKind.Undefined)
            {
                throw new JsonException("Invalid geojson object, does not have 'features' field.");
            }

            var features = featuresProperty.Value.EnumerateArray();

            while (features.MoveNext())
            {
                var feature = features.Current;

                featureCollection.Add(feature.ToObject<Feature>(options));
            }

            return new FeatureCollection(featureCollection);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, FeatureCollection value, JsonSerializerOptions options)
        {
            var featureCollection = value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(featureCollection.Type));
            writer.WriteStringValue(featureCollection.Type.ToString());

            writer.WritePropertyName(nameof(featureCollection.Features));
            writer.WriteStartArray();

            foreach (var feature in featureCollection.Features)
            {
                JsonSerializer.Serialize(writer, feature, options);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}