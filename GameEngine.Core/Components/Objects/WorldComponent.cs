using System.Collections.ObjectModel;
using System.Numerics;

namespace GameEngine.Core.Components.Objects;

public abstract class WorldComponent
{
	public int Id { get; }
	public WorldObject Parent { get; }

	protected Transform Transform => Parent.Transform;
	protected ObservableCollection<MeshData> Meshes => Parent.Meshes;
	protected ObservableCollection<Vector3> Forces => Parent.Forces;
	protected Vector3 Velocity => Parent.Velocity;

	public WorldComponent(WorldObject parent)
	{
		Id = IdGenerator.GenerateNext();
		Parent = parent;

		parent.components.Add(this);
	}
}