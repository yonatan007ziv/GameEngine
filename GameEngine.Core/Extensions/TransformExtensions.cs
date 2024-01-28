using GameEngine.Core.Components;

namespace GameEngine.Core.Extensions;

public static class TransformExtensions
{
	public static TransformData TranslateTransform(this Transform transform)
		=> new TransformData(transform.Position, transform.Rotation, transform.Scale);

	public static Transform TranslateTransform(this TransformData transformData)
		=> new Transform().CopyFrom(transformData);
}