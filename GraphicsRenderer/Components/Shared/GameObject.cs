using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Components.Shared;

public class GameObject : IDisposable
{
	private readonly IGameObjectManager gameObjectManager;

	public int Id { get; }

	public Material? Material { get; set; }
	public Transform Transform { get; }
	public List<IMesh> Meshes = new List<IMesh>();
	public List<IComponent> Components = new List<IComponent>();

	public GameObject(IGameObjectManager gameObjectManager, int id)
	{
		this.gameObjectManager = gameObjectManager;
		Id = id;
		Transform = new Transform();
	}

	public void Render(ICamera camera)
	{
		foreach (IMesh mesh in Meshes)
			if (Material != null)
				mesh.Render(camera, Material);
	}

	public void Update(float deltaTime)
	{
		foreach (IMesh mesh in Meshes)
			mesh.Update(Transform);

		foreach (IComponent component in Components)
			component.Update(deltaTime);
	}

	public void Dispose()
	{
		gameObjectManager.GameObjects.Remove(this);
    }
}