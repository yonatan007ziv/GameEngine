namespace OpenGLRenderer.OpenGL;

internal class Texture
{
	public int Width { get; private set; }
	public int Height { get; private set; }
	public byte[] Data { get; private set; }

	public Texture(int width, int height, byte[] data)
	{
		Width = width;
		Height = height;
		Data = data;
	}
}