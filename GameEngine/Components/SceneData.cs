using GameEngine.Core.Components;

namespace GameEngine.Components;

internal class SceneData
{
	public List<WorldObjectData> GameObjects { get; set; } = new List<WorldObjectData>();
}