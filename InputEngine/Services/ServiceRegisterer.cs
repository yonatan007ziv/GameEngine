using GameEngine.Core.API;
using GameEngine.Core.SharedServices.Implementations.Loggers;
using InputEngine.Services.Implementations.Windows;
using InputEngine.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace InputEngine.Services;

internal class ServiceRegisterer
{
    private readonly IServiceCollection collection;

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
        collection.AddSingleton<IInputEngine, Implementations.InputEngine>();
        collection.AddSingleton<ILogger, ConsoleLogger>();

        // Platform specific
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            RegisterWindowsServices();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            RegisterLinuxServices();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            RegisterMacServices();
        else
        {
            Console.WriteLine("InputEngine: Platform not supported!");
            Environment.Exit(1);
        }
    }

    private void RegisterWindowsServices()
    {
        collection.AddSingleton<IClipboardManager, ClipboardManagerWindows>();
    }

    private void RegisterLinuxServices()
    {
        // Not yet implemented
        throw new NotImplementedException();
    }

    private void RegisterMacServices()
    {
        // Not yet implemented
        throw new NotImplementedException();
    }
}