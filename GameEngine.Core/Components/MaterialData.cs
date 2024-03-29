namespace GameEngine.Core.Components;

public readonly struct MaterialData
{
	public string Shader { get; }
	public string Texture { get; }
	public bool TileTexture { get; }

	public MaterialData(string shader, string texture, bool tileTexture)
	{
		Shader = shader;
		Texture = texture;
		TileTexture = tileTexture;
	}
}