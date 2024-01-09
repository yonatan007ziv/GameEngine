using GameEngine.Core.Components;

namespace PhysicsEngine
{
	public class Program
	{
		public static void Main() { }

		public static void ModifyTransform(Transform transform)
		{
			transform.Scale += new System.Numerics.Vector3(1);
		}
	}
}