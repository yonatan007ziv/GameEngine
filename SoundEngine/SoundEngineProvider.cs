using GameEngine.Core.API;
using Microsoft.Extensions.DependencyInjection;
using SoundEngine.Services;

namespace SoundEngine;

public class SoundEngineProvider
{
    public static ISoundEngine BuildEngine()
        => new ServiceRegisterer().BuildProvider().GetRequiredService<ISoundEngine>();

    public static void RegisterEngine(IServiceCollection collection)
        => new ServiceRegisterer(collection);
}