Provides extensions for creating keyed services using a `Type` for a key with **Microsoft.Extensions.DependencyInjection** and Structure Map.

The expected integration point is to replace `IServiceProviderFactory<Registry>` with `StructureMapKeyedServiceProviderFactory` instead
of the default `StructureMapServiceProviderFactory` implemented provided by Structure Map.

## Commonly Used Types

- StructureMap.StructureMapKeyedServiceProviderFactory

## Release Notes

