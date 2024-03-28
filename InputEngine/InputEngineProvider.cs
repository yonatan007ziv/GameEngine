using GameEngine.Core.API;
using InputEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InputEngine;

public class InputEngineProvider
{
    public static IInputEngine BuildEngine()
        => new ServiceRegisterer().BuildProvider().GetRequiredService<IInputEngine>();

    public static void RegisterEngine(IServiceCollection collection)
        => new ServiceRegisterer(collection);
}