using GameEngine.Core.Components.Fonts;
using GameEngine.Core.SharedServices.Implementations;
using GameEngine.Core.SharedServices.Implementations.FileReaders;
using GameEngine.Core.SharedServices.Implementations.FontParsers;
using GameEngine.Core.SharedServices.Implementations.Loggers;
using GameEngine.Core.SharedServices.Interfaces;
using GameEngine.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameEngine.Services;

internal class ServiceRegisterer
{
    private readonly IServiceCollection collection = new ServiceCollection();
    private bool registeredGraphicsEngine;

    public IServiceProvider BuildProvider()
    {
        RegisterServices();
        return collection.BuildServiceProvider();
    }

    private void RegisterServices()
    {
        collection.AddSingleton<ILogger, ConsoleLogger>();

        RegisterEngines();
    }

    private void RegisterEngines()
    {
        if (!registeredGraphicsEngine)
        {
            Console.WriteLine("No graphics api registered");
            Environment.Exit(1);
        }

        // Shared
        collection.AddSingleton<IResourceDiscoverer, ResourceDiscoverer>();

        // Font parsers
        collection.AddSingleton<IFileReader<Font>, FontFileReader>();
        collection.AddSingleton<TrueTypeFontFileReader>();

        collection.AddSingleton<IGameEngine, Implementations.GameEngine>();
        PhysicsEngine.PhysicsEngineProvider.RegisterEngine(collection);
        SoundEngine.SoundEngineProvider.RegisterEngine(collection);
        InputEngine.InputEngineProvider.RegisterEngine(collection);
    }

    public void UseOpenTK()
    {
        GraphicsEngine.GraphicsEngineProvider.RegisterEngineOpenTK(collection);
        registeredGraphicsEngine = true;
    }

    public void UseSilkOpenGL()
    {
        GraphicsEngine.GraphicsEngineProvider.RegisterEngineSilkOpenGL(collection);
        registeredGraphicsEngine = true;
    }
}