using System.Numerics;

namespace GameEngine.Core.Components;

public class BoxData
{
	public Vector3 Postion { get; set; }
	public Vector3 Scale { get; set; }

	public BoxData(Vector3 postion, Vector3 scale)
	{
		Postion = postion;
		Scale = scale;
	}
}