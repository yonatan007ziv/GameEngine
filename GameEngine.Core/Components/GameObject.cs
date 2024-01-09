namespace GameEngine.Core.Components;

public class GameObject
{
	public int Id { get; }

	public Transform Transform { get; }
	//public List<IMeshRenderer> Meshes = new List<IMeshRenderer>();
	//public List<IComponent> Components = new List<IComponent>();

	public GameObject(int id)
	{
		Id = id;
		Transform = new Transform();
	}

	public void Update(float deltaTime)
	{
		//foreach (IMeshRenderer mesh in Meshes)
		//	mesh.Update(Transform);

		//foreach (IComponent component in Components)
		//	component.Update(deltaTime);
	}
}