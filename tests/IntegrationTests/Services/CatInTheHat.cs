// Copyright (c) Commonsense Software and contributors. All rights reserved.

#pragma warning disable SA1135 // Using directives should be qualified

namespace More.Extensions.DependencyInjection.Services;

using Thing_1 = IKeyed<Key.Thing1, IThing>;
using Thing_2 = IKeyed<Key.Thing2, IThing>;

public class CatInTheHat
{
    public CatInTheHat( Thing_1 thing1, Thing_2 thing2 )
    {
        ArgumentNullException.ThrowIfNull( thing1, nameof( thing1 ) );
        ArgumentNullException.ThrowIfNull( thing2, nameof( thing2 ) );

        Thing1 = thing1.Value;
        Thing2 = thing2.Value;
    }

    public IThing Thing1 { get; }

    public IThing Thing2 { get; }
}