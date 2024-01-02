namespace GraphicsRenderer.Components.Shared;

public class ShaderSource
{
	public string Source { get; private set; }

	public ShaderSource(string source)
	{
		Source = source;
	}
}