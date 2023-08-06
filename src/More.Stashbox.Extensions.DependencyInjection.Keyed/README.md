Provides extensions for creating keyed services using a `Type` for a key with **Microsoft.Extensions.DependencyInjection** and Stashbox.

The expected integration point is to replace `IServiceProviderFactory<IStashboxContainer>` with `StashboxKeyedServiceProviderFactory`
instead of the default `StashboxServiceProviderFactory` implemented provided by Stashbox.

## Commonly Used Types

- Stashbox.Extensions.DependencyInjection.StashboxKeyedServiceProviderFactory

## Release Notes

