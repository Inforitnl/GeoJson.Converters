# Changelog

All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0]

### Added

- Added custom converters for every BAMCIS.GeoJson type
- Added an extension to return a list of all the converters
- Added test project (MultiPolygonTest fails due to string compare, result of System.Text.Json double deserialization removes precision if only .0)
- Added extension method to add the converters to the JsonSerializerOptions

### Changed

- The order of the values in the array to confirm to GeoJson [longitude, latitude]

### Removed

- n.a.
