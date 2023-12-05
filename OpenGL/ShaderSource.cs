namespace OpenGLRenderer.OpenGL;

internal class ShaderSource
{
	private readonly string shaderSource;

	public ShaderSource(string shaderName)
	{
		shaderSource = LoadSource(shaderName);
	}

	private string LoadSource(string shaderName)
	{
		string path = @"D:\Code\VS Community\OpenGLRenderer\OpenGL\Shaders";
		return File.ReadAllText(Path.Combine(path, shaderName));
	}

	public string GetSource()
	{
		return shaderSource;
	}
}