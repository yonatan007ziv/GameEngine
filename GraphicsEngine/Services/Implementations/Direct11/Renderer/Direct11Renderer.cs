using GameEngine.Core.API;
using GameEngine.Core.Components;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.Direct11.Renderer;

public class Direct11Renderer : IGraphicsEngine
{
	private readonly ILogger logger;

	public string Title { get; set; }

	public List<GameObject> RenderingObjects => throw new NotImplementedException();

	public Transform CameraTransform => throw new NotImplementedException();

	public Direct11Renderer(ILogger logger)
	{
		this.logger = logger;
	}

	public void Start()
	{
		logger.LogInformation("Direct11 Renderer Started");
	}

	public void LockMouse(bool lockMouse)
	{

	}

	public void TurnVSync(bool vsync)
	{

	}

	public void Render()
	{
		throw new NotImplementedException();
	}

	public void SyncState()
	{
		throw new NotImplementedException();
	}

	public void RenderFrame()
	{
		throw new NotImplementedException();
	}
}