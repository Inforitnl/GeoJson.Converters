using System.Text.Json;

using BAMCIS.GeoJSON;

namespace Inforit.GeoJson.Converters.Extensions
{
    /// <summary>
    /// Utf8JsonWriter extensions
    /// </summary>
    public static class Utf8JsonWriterExtensions
    {
        /// <summary>
        /// Write a Position object
        /// </summary>
        /// <param name="writer"><see cref="Utf8JsonWriter" /></param>
        /// <param name="position"><see cref="Position" /></param>
        public static void WritePosition(this Utf8JsonWriter writer, Position position)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(position.Longitude);
            writer.WriteNumberValue(position.Latitude);
            writer.WriteEndArray();
        }
    }
}