using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inforit.GeoJson.Converters.Extensions
{
    /// <summary>
    /// Extensions for the GeoJson converters.
    /// </summary>
    public static class GeoJsonConverterExtensions
    {
        /// <summary>
        /// Return a list of JsonConverters for GeoJson support.
        /// <para>Default System.Text.Json:</para>
        /// <list type="bullet">
        /// <item>
        /// JsonStringEnumConverter(JsonNamingPolicy.CamelCase, true)
        /// </item> 
        /// <para>GeoJson:</para>
        /// <item>
        /// FeatureCollectionConverter
        /// </item>
        /// <item>
        /// FeatureConverter
        /// </item>
        /// <item>
        /// FeatureIdConverter
        /// </item>
        /// <item>
        /// GeoJsonConverter
        /// </item>
        /// <item>
        /// GeometryCollectionConverter
        /// </item>
        /// <item>
        /// GeometryConverter
        /// </item>
        /// <item>
        /// LineStringConverter
        /// </item>
        /// <item>
        /// MultiLineStringConverter
        /// </item>
        /// <item>
        /// MultiPointConverter
        /// </item>
        /// <item>
        /// MultiPolygonConverter
        /// </item>
        /// <item>
        /// PointConverter
        /// </item>
        /// <item>
        /// PolygonConverter
        /// </item>
        /// <item>
        /// PositionConverter
        /// </item>
        /// </list>
        /// </summary>
        public static IEnumerable<JsonConverter> Converters()
        {
            return new List<JsonConverter>
            {
                // default System.Text.Json converter for enums
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, true),
                new FeatureCollectionConverter(),
                new FeatureConverter(),
                new FeatureIdConverter(),
                new GeoJsonConverter(),
                new GeometryCollectionConverter(),
                new GeometryConverter(),
                new LineStringConverter(),
                new MultiLineStringConverter(),
                new MultiPointConverter(),
                new MultiPolygonConverter(),
                new PointConverter(),
                new PolygonConverter(),
                new PositionConverter()
            };
        }

        /// <summary>
        /// Extension method to add the converters to the <see cref="JsonSerializerOptions"/>
        /// </summary>
        /// <param name="options">The <see cref="JsonSerializerOptions"/> to add the converter to</param>
        public static void AddConverters(this JsonSerializerOptions options)
        {
            foreach (var converter in Converters())
            {
                options.Converters.Add(converter);
            }
        }
    }
}