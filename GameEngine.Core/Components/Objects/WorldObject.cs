namespace GameEngine.Core.Components.Objects;

public class WorldObject : GameObject
{
	public WorldObject() { }
	public WorldObject(WorldObject parent) : base(parent) { }
}