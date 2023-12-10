namespace OpenGLRenderer.OpenGL;

internal class ShaderSource
{
	public string Source { get; private set; }

	public ShaderSource(string shaderName)
	{
		Source = LoadSource(shaderName);
	}

	private string LoadSource(string shaderName)
	{
		string path = @"D:\Code\VS Community\OpenGLRenderer\Resources\Shaders";
		return File.ReadAllText(Path.Combine(path, shaderName));
	}
}