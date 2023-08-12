namespace Keyed.DI.Example;

public sealed class ServiceA : IService
{
    public Task<string> DoAsync() => Task.FromResult( "Hello from Service A" );
}