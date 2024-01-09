using GameEngine.Core.API;
using GameEngine.Services.Interfaces.Managers;
using Microsoft.Extensions.Logging;

namespace GameEngine;

internal class GameEngine
{
	private readonly ILogger logger;
	private readonly IGraphicsEngine renderer;
	private readonly IGameObjectManager gameObjectManager;

	public GameEngine(ILogger logger, IGraphicsEngine renderer, IGameObjectManager gameObjectManager)
	{
		this.logger = logger;
		this.renderer = renderer;
		this.gameObjectManager = gameObjectManager;

		Start();
	}

	private void Start()
	{
		renderer.RenderingObjects.Add(gameObjectManager.CreateGameObject());
	}

	public void Run()
	{
		renderer.TurnVSync(true);
		renderer.LockMouse(false);

		while (true)
		{
			SyncRenderer();
			renderer.RenderFrame();
		}
	}

	private void SyncRenderer()
	{
		// probably use IGameObjectManager to track changes and or use a message queue for GameObject id
		renderer.SyncState();
	}
}