using GameEngine.Core.Components.CommunicationComponentsData;

namespace PhysicsEngine
{
	public class Program
	{
		public static void Main() { }

		public static void ResolveCollision(ref TransformData transform)
		{
			transform.Scale += new System.Numerics.Vector3(1);
		}
	}
}