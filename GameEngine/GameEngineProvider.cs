using GameEngine.Services;
using GameEngine.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameEngine;

public class GameEngineProvider
{
    private readonly ServiceRegisterer serviceRegisterer = new ServiceRegisterer();

    public IGameEngine BuildEngine()
        => serviceRegisterer.BuildProvider().GetRequiredService<IGameEngine>();

    public GameEngineProvider UseSilkOpenGL()
    {
        serviceRegisterer.UseSilkOpenGL();
        return this;
    }

    public GameEngineProvider UseOpenTK()
    {
        serviceRegisterer.UseOpenTK();
        return this;
    }
}