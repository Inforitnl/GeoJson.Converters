# Inforit.GeoJson.Converters NuGet package

[![logo](./logo.jpg)](https://inforit.nl)

This package contains converters and extensions to have support for [BAMCIS.GeoJSON](https://github.com/bamcis-io/GeoJSON). This package is intended to be used when the conversion of BAMCIS.GeoJSON objects with System.Text.Json is required.

## Using converter extension

```csharp
services
    .AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseRoutingConvention())))
    .AddJsonOptions(options => options.JsonSerializerOptions.AddConverters());
```

## Contributor information

When making a PR please have a look at the [CONTRIBUTING] document in this repository.
