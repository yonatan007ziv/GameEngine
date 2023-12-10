namespace OpenGLRenderer.OpenGL;

internal class TextureSource
{
	public byte[] Source { get; }
	public int Width { get; }
	public int Height { get; }

	public TextureSource(byte[] source, int width, int height)
	{
		Source = source;
		Width = width;
		Height = height;
	}
}