using System.Collections.ObjectModel;
using System.Numerics;

namespace GameEngine.Core.Components.Objects;

public class WorldObject
{
	public int Id { get; }
	public string Tag { get; protected set; } = "";

	public readonly List<WorldComponent> components = new List<WorldComponent>();

	public Transform Transform { get; }
	public BoxCollider? BoxCollider { get; set; }
	public ObservableCollection<MeshData> Meshes { get; }
	public ObservableCollection<Vector3> Forces { get; }
	public ObservableCollection<Vector3> ImpulseVelocities { get; }

	public WorldObject()
	{
		Id = IdGenerator.GenerateNext();

		Transform = new Transform();
		Meshes = new ObservableCollection<MeshData>();
		Forces = new ObservableCollection<Vector3>();
		ImpulseVelocities = new ObservableCollection<Vector3>();
	}
}