namespace Keyed.DI.Example;

public sealed class ServiceB : IService
{
    public Task<string> DoAsync() => Task.FromResult( "Hello from Service B" );
}