// Copyright (c) Commonsense Software and contributors. All rights reserved.

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

namespace More.Extensions.DependencyInjection.Services;

public abstract class ThingBase : IThing
{
    protected ThingBase() { }

    public override string ToString() => GetType().Name;
}

public sealed class Thing : ThingBase { }

public sealed class KeyedThing : ThingBase { }

public sealed class Thing1 : ThingBase { }

public sealed class Thing2 : ThingBase { }

public sealed class Thing3 : ThingBase { }