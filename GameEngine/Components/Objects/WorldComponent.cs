using GameEngine.Core.Components;
using System.Collections.ObjectModel;
using System.Numerics;

namespace GameEngine.Components.Objects;

public abstract class WorldComponent : IDisposable
{
	public int Id { get; }
	public int ParentId { get; }

	protected Transform Transform;
	protected ObservableCollection<MeshData> Meshes;
	protected ObservableCollection<Vector3> Forces;
	protected ObservableCollection<Vector3> ImpulseVelocities;

	public WorldComponent(WorldObject parent)
	{
		Id = IdGenerator.GenerateNext();
		ParentId = parent.Id;

		parent.components.Add(this);

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