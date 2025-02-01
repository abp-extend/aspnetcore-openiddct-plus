using AspNetCoreOpeniddictPlus.InertiaCore.Models;

namespace AspNetCoreOpeniddictPlus.InertiaCore.Props;

public class AlwaysProp : InvokableProp
{
    internal AlwaysProp(object? value) : base(value)
    {
    }

    internal AlwaysProp(Func<object?> value) : base(value)
    {
    }

    internal AlwaysProp(Func<Task<object?>> value) : base(value)
    {
    }
}