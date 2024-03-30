namespace GameEngine.Core.Components;

public class CameraRenderingMask<T>
{
	public readonly ICollection<T> RenderingMask = new List<T>();
	public bool InMask(T maskElement) => RenderingMask.Contains(maskElement);
}