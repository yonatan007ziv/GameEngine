
using GameEngine.Core.API;
using Microsoft.Extensions.DependencyInjection;

namespace PhysicsEngine.Services;

internal class ServiceRegisterer
{
    private readonly IServiceCollection collection = new ServiceCollection();

    public ServiceRegisterer()
    {
        collection = new ServiceCollection();
        RegisterServices();
    }

    public ServiceRegisterer(IServiceCollection collection)
    {
        this.collection = collection;
        RegisterServices();
    }

    public IServiceProvider BuildProvider()
        => collection.BuildServiceProvider();

    private void RegisterServices()
    {
        collection.AddSingleton<IPhysicsEngine, Implementations.PhysicsEngine>();
    }
}