# Changelog

All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.3]

- Added extension method to add the converters to the JsonSerializerOptions

## [1.0.2]

- Added custom converters for every BAMCIS.GeoJson type
- Added an extension to return a list of all the converters
- Added test project (MultiPolygonTest fails due to string compare, result of System.Text.Json double deserialization removes precision if only .0)
- Removed GeoJsonCapSerializer in favor of to the new converters
- For the time being changed the bitbucket-pipelines.yml to only look at the actual .csproj and not to the .Tests.csproj

## [1.0.1]

### Changed

- The order of the values in the array to confirm to GeoJson [longitude, latitude]

## [1.0.0]

### Added

- Added entities to store the GeoJson
- Added model to represent a GeoJson to consumer
- Added extension to add the serializer from package to your project
- Added helper class for a Polygon

### Changed

- n.a.

### Removed

- n.a.
