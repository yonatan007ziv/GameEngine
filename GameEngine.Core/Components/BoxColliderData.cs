using System.Numerics;

namespace GameEngine.Core.Components;

public readonly struct BoxColliderData
{
	public bool StaticCollider { get; }

	public Vector3 Min { get; }
	public Vector3 Max { get; }

	public BoxColliderData(bool staticCollider, Vector3 min, Vector3 max)
	{
		StaticCollider = staticCollider;
		Min = min;
		Max = max;
	}
}