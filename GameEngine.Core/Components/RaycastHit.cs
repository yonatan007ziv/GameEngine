using System.Numerics;

namespace GameEngine.Core.Components;

public struct RaycastHit
{
	public Vector3 point;
	public float distance;
	public int gameObjectHitId;
}