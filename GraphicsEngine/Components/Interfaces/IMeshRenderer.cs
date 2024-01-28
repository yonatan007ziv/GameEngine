using GameEngine.Core.Components;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;

namespace GraphicsEngine.Components.Interfaces;

internal interface IMeshRenderer
{
	ModelData Model { get; set; }
	Material Material { get; set; }

	public void Render(Camera camera);
	public void Update(Transform transform);
}