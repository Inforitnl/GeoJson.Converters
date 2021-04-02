# Inforit.GeoJson.Converters NuGet package

[![logo](./logo.jpg)](https://inforit.nl)

This package contains converters and extensions to have support for BAMCIS.GeoJSON. This package is intended to be used when the conversion of BAMCIS.GeoJSON objects with System.Text.Json is required.

## Using converter extension

```csharp
services
    .AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseRoutingConvention())))
    .AddJsonOptions(options => options.JsonSerializerOptions.AddConverters());
```

## Getting started

Keep in mind you maintain the version (following semver) in the project file.

## Build and publish

After the pipeline is configured for the repository the pipeline will automatically take care of this.
Commits in the master branch will produce stable packages. Builds in all other branches will produce unstable packages. These unstable packages will have a buildnumber suffix.

### Publish configuration

Configuration of the package feed is done with environment variables:

- Package feed uri: `$MYGET_PUBLISH_PACKAGE_SOURCE`
- Package feed api key: `$MYGET_ACCESS_TOKEN`

### Sonarcloud

For code analysis the created project has to be configured in Sonarcloud and Bitbucket. The build pipeline is pre-configured to support this code analysis. To disable this just comment out the related pipelines (not recommended).

Connecting to Sonarcloud requires the following environment variables to be set:

- Sonarcloud project id: `$SONAR_PROJECT`
- Sonarcloud project specific token: `$SONAR_TOKEN`
