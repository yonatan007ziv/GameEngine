using GameEngine.Core.Components;
using System.Collections.ObjectModel;
using System.Numerics;

namespace GameEngine.Components;

public abstract class GameComponent : IDisposable
{
	public int Id { get; }
	public int ParentId { get; }
	public bool IsUI { get; set; }

	protected Transform Transform;
	protected ObservableCollection<MeshData> Meshes;
	protected ObservableCollection<Vector3> Forces;
	protected ObservableCollection<Vector3> ImpulseVelocities;

	public GameComponent(GameObject parent)
    {
		Id = IdGenerator.GenerateNext();
		ParentId = parent.Id;

		parent.gameComponents.Add(this);

		Transform = parent.Transform;
		Meshes = parent.Meshes;
		Forces = parent.Forces;
		ImpulseVelocities = parent.ImpulseVelocities;
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}
}