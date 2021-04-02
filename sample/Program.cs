using System.Globalization;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using BAMCIS.GeoJSON;

using Inforit.GeoJson.Converters;
using Inforit.GeoJson.Converters.Extensions;

namespace Inforit.GeoJson.Converters.sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sampleJson = @"{
                ""type"": ""Feature"",
                ""geometry"": {
                    ""type"": ""Point"",
                    ""coordinates"": [51.7543453, 5.550476]
                },
                ""properties"": {
                    ""name"": ""Inforit""
                }
            }";

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.AddConverters();

            var feature = JsonSerializer.Deserialize<Feature>(sampleJson, serializerOptions);

            Console.WriteLine("Feature: " + feature.Type);
            Console.WriteLine("Coordinates: " + ((Point)feature.Geometry).Coordinates);
        }
    }
}
