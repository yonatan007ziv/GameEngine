namespace GraphicsRenderer.Components.Shared;

internal class ShaderSource
{
	public string Source { get; private set; }

	public ShaderSource(string source)
	{
		Source = source;
	}
}