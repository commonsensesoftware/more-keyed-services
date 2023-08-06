Provides extensions for creating keyed services using a `Type` for a key with **Microsoft.Extensions.DependencyInjection** and Lamar.

The expected integration point is to replace `IServiceProviderFactory<ServiceRegistry>` with `LamarKeyedServiceProviderFactory` instead
of the default `LamarServiceProviderFactory` implemented provided by Lamar.

## Commonly Used Types

- Lamar.Microsoft.DependencyInjection.LamarKeyedServiceProviderFactory

## Release Notes

