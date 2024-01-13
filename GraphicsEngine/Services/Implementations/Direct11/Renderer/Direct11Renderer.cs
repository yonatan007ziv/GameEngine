using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.Direct11.Renderer;

public class Direct11Renderer // : IGraphicsEngine
{
	private readonly ILogger logger;

	public Direct11Renderer(ILogger logger)
	{
		this.logger = logger;
	}

	public void Start()
	{
		logger.LogInformation("Direct11 Renderer Started");
	}
}