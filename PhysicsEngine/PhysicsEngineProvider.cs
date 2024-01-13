using GameEngine.Core.API;
using Microsoft.Extensions.DependencyInjection;
using PhysicsEngine.Services;

namespace PhysicsEngine;

public class PhysicsEngineProvider
{
	public static IPhysicsEngine BuildEngine()
		=> new ServiceRegisterer().BuildProvider().GetRequiredService<IPhysicsEngine>();
}