namespace GameEngine.Core.Components;

public readonly struct MaterialData
{
	public string Shader { get; }
	public string Texture { get; }

	public MaterialData(string shader, string texture)
	{
		Shader = shader;
		Texture = texture;
	}
}