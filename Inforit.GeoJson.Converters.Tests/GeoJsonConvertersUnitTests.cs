using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using BAMCIS.GeoJSON;

using Inforit.GeoJson.Converters;

using Xunit;

namespace Inforit.GeoJson.Converters.Tests
{
    public class GeoJsonUnitTests
    {
        public static JsonSerializerOptions Options => new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = {
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
                    new PositionConverter(),
                    new JsonStringEnumConverter()
                }
        };

        [Fact]
        public void FeatureTest()
        {
            // ARRANGE
            var content = ReadAllText("feature.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Feature>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<Feature>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureCollectionTest()
        {
            // ARRANGE
            var content = ReadAllText("featurecollection.json");

            // ACT
            var geo = JsonSerializer.Deserialize<FeatureCollection>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<FeatureCollection>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureIdEqualTest()
        {
            // ARRANGE
            var content = ReadAllText("feature_id_number.json");

            // ACT
            var geo1 = JsonSerializer.Deserialize<Feature>(content, Options);
            var geo2 = JsonSerializer.Deserialize<Feature>(content, Options);

            // ASSERT
            Assert.Equal(geo1.Id.Value, geo2.Id.Value);
            Assert.Equal(geo1.Id, geo2.Id);
        }

        [Fact]
        public void FeatureIdNotEqualTest()
        {
            // ARRANGE
            var content1 = ReadAllText("feature_id_number.json");
            var content2 = ReadAllText("feature_id_num_as_string.json");

            // ACT
            var geoWithNumId = JsonSerializer.Deserialize<Feature>(content1, Options);
            var geoWithStringId = JsonSerializer.Deserialize<Feature>(content2, Options);

            // ASSERT
            Assert.Equal(geoWithNumId.Id.Value, geoWithStringId.Id.Value);
            Assert.NotEqual(geoWithNumId.Id, geoWithStringId.Id);
        }

        [Fact]
        public void FeatureOutOfRangeTest()
        {
            // ARRANGE
            GeoJsonConfig.EnforcePositionValidation();
            var content = ReadAllText("feature_out_of_range.json");

            // ACT & ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => JsonSerializer.Deserialize<Feature>(content, Options));
        }

        [Fact]
        public void FeatureOutOfRangeTestIgnoreValidation()
        {
            // ARRANGE
            GeoJsonConfig.IgnorePositionValidation();
            var content = ReadAllText("feature_out_of_range.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Feature>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<Feature>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureTestNullGeometry()
        {
            // ARRANGE
            var content = ReadAllText("feature_null_geometry.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Feature>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<Feature>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureTestNumberId()
        {
            // ARRANGE
            var content = ReadAllText("feature_id_number.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Feature>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<Feature>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureTestStringId()
        {
            // ARRANGE
            var content = ReadAllText("feature_id_string.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Feature>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<Feature>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureTestWithBbox()
        {
            // ARRANGE
            var content = ReadAllText("featurebbox.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Feature>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<Feature>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeoJsonTest()
        {
            // ARRANGE
            //var content = ReadAllText("feature.json");
            //var content = ReadAllText("featurecollection.json");
            //var content = ReadAllText("featurecollectionbbox.json");
            var content = ReadAllText("geometrycollection.json");


            // ACT
            var geo = JsonSerializer.Deserialize<BAMCIS.GeoJSON.GeoJson>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<BAMCIS.GeoJSON.GeoJson>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeoJsonFeatureTest()
        {
            // ARRANGE
            var content = ReadAllText("feature.json");

            // ACT
            var geo = BAMCIS.GeoJSON.GeoJson.FromJson(content);
            var content2 = geo.ToJson();
            var geo2 = BAMCIS.GeoJSON.GeoJson.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeoJsonFeatureTestWithBbox()
        {
            // ARRANGE
            var content = ReadAllText("featurebbox.json");

            // ACT
            var geo = BAMCIS.GeoJSON.GeoJson.FromJson(content);
            var content2 = geo.ToJson();
            var geo2 = BAMCIS.GeoJSON.GeoJson.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeoJson3DLinevarTestWithBbox()
        {
            // ARRANGE
            var content = ReadAllText("3dlinestringbbox.json");

            // ACT
            var geo = BAMCIS.GeoJSON.GeoJson.FromJson(content);
            var content2 = geo.ToJson();
            var geo2 = BAMCIS.GeoJSON.GeoJson.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeoJsonFeatureCollectionTestWithBbox()
        {
            // ARRANGE
            var content = ReadAllText("featurecollectionbbox.json");

            // ACT
            var geo = BAMCIS.GeoJSON.GeoJson.FromJson(content);
            var content2 = geo.ToJson();
            var geo2 = BAMCIS.GeoJSON.GeoJson.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeometryCollectionTest()
        {
            // ARRANGE
            var content = ReadAllText("geometrycollection.json");

            // ACT
            var geo = JsonSerializer.Deserialize<GeometryCollection>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<GeometryCollection>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void LineStringTest()
        {
            // ARRANGE
            var content = ReadAllText("linestring.json");

            // ACT
            var geo = JsonSerializer.Deserialize<LineString>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<LineString>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void MultiPointTest()
        {
            // ARRANGE
            var content = ReadAllText("multipoint.json");

            // ACT
            var geo = JsonSerializer.Deserialize<MultiPoint>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<MultiPoint>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void MultiLineStringTest()
        {
            // ARRANGE
            var content = ReadAllText("multilinestring.json");

            // ACT
            var geo = MultiLineString.FromJson(content);
            var content2 = geo.ToJson();
            var geo2 = MultiLineString.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }


        [Fact]
        public void MultiPolygonTest()
        {
            // ARRANGE
            var content = ReadAllText("multipolygon.json");

            // ACT
            var geo = JsonSerializer.Deserialize<MultiPolygon>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);

            // ASSERT
            Assert.Equal(content, content2, true, true, true);
        }

        [Fact]
        public void PointTest()
        {
            // ARRANGE
            var content = ReadAllText("point.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Point>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<Point>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void PolygonTest()
        {
            // ARRANGE
            var content = ReadAllText("polygon.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Polygon>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<Polygon>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void PolygonWithHoleTest()
        {
            // ARRANGE
            var content = ReadAllText("polygonwithhole.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Polygon>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);
            var geo2 = JsonSerializer.Deserialize<Polygon>(content2, Options);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void PolygonRemoveInnerRingsTestWithHole()
        {
            // ARRANGE
            var content = ReadAllText("polygonwithhole.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Polygon>(content, Options);
            var result = geo.RemoveInteriorRings();

            // ASSERT
            Assert.True(result);
            Assert.Single(geo.Coordinates);
        }

        [Fact]
        public void PolygonRemoveInnerRingsTestWithoutHole()
        {
            // ARRANGE
            var content = ReadAllText("polygon.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Polygon>(content, Options);
            var result = geo.RemoveInteriorRings();

            // ASSERT
            Assert.False(result);
            Assert.Single(geo.Coordinates);
        }

        [Fact]
        public void PositionTest()
        {
            // ARRANGE
            var content = ReadAllText("position.json");

            // ACT
            var geo = JsonSerializer.Deserialize<Position>(content, Options);
            var content2 = JsonSerializer.Serialize(geo, Options);

            // ASSERT
            Assert.Equal(content, content2, true, true, true);
        }

        public static string ReadAllText(string filename)
        {
            return File.ReadAllText($"jsons/{filename}").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
        }
    }
}
