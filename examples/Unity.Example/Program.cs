using Keyed.DI.Example;
using Microsoft.AspNetCore.Mvc;
using Unity.Microsoft.DependencyInjection;

// this is a convenience, not a requirement and can be radically simplified in C# 12+
// REF: https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12#alias-any-type
using A = More.Extensions.DependencyInjection.IKeyed<Keyed.DI.Example.Key.A, Keyed.DI.Example.IService>;
using B = More.Extensions.DependencyInjection.IKeyed<Keyed.DI.Example.Key.B, Keyed.DI.Example.IService>;

var builder = WebApplication.CreateBuilder( args );

builder.Host.UseUnityServiceProvider();

builder.Services.AddTransient<Key.A, IService, ServiceA>();
builder.Services.AddTransient<Key.B, IService, ServiceB>();

var app = builder.Build();
var example = app.MapGroup( "/example" );

example.MapGet( "/a", ( [FromServices] A service ) => service.Value.DoAsync() );
example.MapGet( "/b", ( [FromServices] B service ) => service.Value.DoAsync() );

app.Run();