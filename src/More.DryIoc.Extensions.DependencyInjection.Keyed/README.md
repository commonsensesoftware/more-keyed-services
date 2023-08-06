Provides extensions for creating keyed services using a `Type` for a key with **Microsoft.Extensions.DependencyInjection** and Dry IoC.

The expected integration point is to replace `IServiceProviderFactory<IContainer>` with `DryIocKeyedServiceProviderFactory` instead
of the default `DryIocServiceProviderFactory` implemented provided by Dry IoC.

## Commonly Used Types

- DryIoc.Microsoft.DependencyInjection.DryIocKeyedServiceProviderFactory

## Release Notes

