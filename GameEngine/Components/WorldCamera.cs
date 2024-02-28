using GameEngine.Components.Objects;
using GameEngine.Components.Objects.Scriptable;

namespace GameEngine.Components;

public class WorldCamera : ScriptableWorldComponent
{
	public bool Standalone { get; }

	public WorldCamera()
		: base(new WorldObject())
	{
		Standalone = true;
	}

	public WorldCamera(WorldObject parent)
		: base(parent)
	{
		Standalone = false;
	}

	public override void Update(float deltaTime)
	{

	}
}