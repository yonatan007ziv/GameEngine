using GameEngine.Core.API;
using Microsoft.Extensions.DependencyInjection;
using PhysicsEngine.Services;

namespace PhysicsEngine;

public class PhysicsEngineProvider
{
    public static IPhysicsEngine BuildEngine()
        => new ServiceRegisterer().BuildProvider().GetRequiredService<Services.Implementations.PhysicsEngine>();

    public static void RegisterEngine(IServiceCollection collection)
        => new ServiceRegisterer(collection);
}