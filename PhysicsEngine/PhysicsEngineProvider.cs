using GameEngine.Core.API;

namespace PhysicsEngine;

public class PhysicsEngineProvider
{
	public static IPhysicsEngine BuildEngine()
		=> new PhysicsEngine();
}