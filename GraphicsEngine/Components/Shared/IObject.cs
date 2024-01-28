using GameEngine.Core.Components;
using GraphicsEngine.Components.Interfaces;

namespace GraphicsEngine.Components.Shared;

internal interface IObject
{
	public int Id { get; }
	public Transform Transform { get; }
	public List<IMeshRenderer> Meshes { get; }
	public void Render(Camera camera);
	public void Update();
}