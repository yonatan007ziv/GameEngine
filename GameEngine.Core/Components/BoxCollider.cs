using System.Numerics;

namespace GameEngine.Core.Components;

public class BoxCollider
{
	public bool StaticCollider { get; }
	public Vector3 Min { get; }
	public Vector3 Max { get; }
	public Vector3 Size => Max - Min;
	public Vector3 Center => (Max + Min) / 2;

	public BoxCollider(bool staticCollider, Vector3 min, Vector3 max)
	{
		StaticCollider = staticCollider;
		Min = min;
		Max = max;
	}
}