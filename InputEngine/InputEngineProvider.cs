using GameEngine.Core.API;
using InputEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InputEngine;

public class InputEngineProvider
{
	public static IInputEngine BuildEngine()
		=> new ServiceRegisterer().BuildProvider().GetRequiredService<IInputEngine>();
}