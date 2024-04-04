namespace GameEngine.Core.Components.Objects;

public abstract class WorldObject : GameObject
{
	public WorldObject() { }
	public WorldObject(WorldObject parent) : base(parent) { }
}