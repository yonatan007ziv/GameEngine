using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Components.Shared;

public class GameObject : IDisposable
{
	private readonly IGameObjectManager gameObjectManager;

	public int Id { get; }

	public Transform Transform { get; }
	public List<IMeshRenderer> Meshes = new List<IMeshRenderer>();
	public List<IComponent> Components = new List<IComponent>();

	public GameObject(IGameObjectManager gameObjectManager, int id)
	{
		this.gameObjectManager = gameObjectManager;
		Id = id;
		Transform = new Transform();
	}

	public void Render(ICamera camera)
	{
		foreach (IMeshRenderer mesh in Meshes)
			mesh.Render(camera);
	}

	public void Update(float deltaTime)
	{
		foreach (IMeshRenderer mesh in Meshes)
			mesh.Update(Transform);

		foreach (IComponent component in Components)
			component.Update(deltaTime);
	}

	public void Dispose()
	{
		gameObjectManager.GameObjects.Remove(this);
	}
}