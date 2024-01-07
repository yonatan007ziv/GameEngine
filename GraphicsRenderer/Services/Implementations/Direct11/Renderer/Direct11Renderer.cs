using GraphicsRenderer.Services.Interfaces.Renderer;
using Microsoft.Extensions.Logging;

namespace GraphicsRenderer.Services.Implementations.Direct11.Renderer;

public class Direct11Renderer : IRenderer
{
	private readonly ILogger logger;

	public string Title { get; set; }

	public Direct11Renderer(ILogger logger)
	{
		this.logger = logger;
	}

	public void Run()
	{
		logger.LogInformation("Direct11 Renderer Started");
	}

	public void LockMouse(bool lockMouse)
	{

	}

	public void TurnVSync(bool vsync)
	{

	}
}