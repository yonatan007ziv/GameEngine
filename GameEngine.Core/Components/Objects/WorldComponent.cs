using System.Collections.ObjectModel;
using System.Numerics;

namespace GameEngine.Core.Components.Objects;

public abstract class WorldComponent
{
	public int Id { get; }
	public WorldObject Parent { get; }

	protected Transform Transform;
	protected ObservableCollection<MeshData> Meshes;
	protected ObservableCollection<Vector3> Forces;
	protected ObservableCollection<Vector3> ImpulseVelocities;

	public WorldComponent(WorldObject parent)
	{
		Id = IdGenerator.GenerateNext();
		Parent = parent;

		parent.components.Add(this);

		Transform = parent.Transform;
		Meshes = parent.Meshes;
		Forces = parent.Forces;
		ImpulseVelocities = parent.ImpulseVelocities;
	}
}