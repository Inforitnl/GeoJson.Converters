using System;
using System.Buffers;
using System.Text.Json;

namespace Inforit.GeoJson.Converters.Extensions
{
    /// <summary>
    /// Provides extension methods to the <see cref="JsonDocument">JsonDocument</see> and <see cref="JsonElement">JsonElement</see> object.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Deserialize the JsonElement into an instance of the type
        /// specified by a generic type parameter.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                element.WriteTo(writer);
            }

            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }

        /// <summary>
        /// Deserialize the JsonDocument into an instance of the type
        /// specified by a generic type parameter.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObject<T>(this JsonDocument document, JsonSerializerOptions options = null)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return document.RootElement.ToObject<T>(options);
        }

        /// <summary>
        /// Deserialize the JsonElement into an object of the type
        /// specified by the return type parameter.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object ToObject(this JsonElement element, Type returnType, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                element.WriteTo(writer);
            }

            return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, returnType, options);
        }

        /// <summary>
        /// Deserialize the JsonDocument into an object of the type
        /// specified by the return type parameter.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object ToObject(this JsonDocument document, Type returnType, JsonSerializerOptions options = null)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return document.RootElement.ToObject(returnType, options);
        }
    }
}