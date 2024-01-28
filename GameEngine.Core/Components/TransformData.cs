using System.Numerics;

namespace GameEngine.Core.Components;

public readonly struct TransformData
{
	public readonly Vector3 position, rotation, scale;

	public TransformData(Vector3 position, Vector3 rotation, Vector3 scale)
	{
		this.position = position;
		this.rotation = rotation;
		this.scale = scale;
	}
}