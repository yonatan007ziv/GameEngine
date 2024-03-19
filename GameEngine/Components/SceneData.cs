using GameEngine.Core.Components.Objects;

namespace GameEngine.Components;

internal class SceneData
{
	public List<WorldObject> GameObjects { get; set; } = new List<WorldObject>();
}