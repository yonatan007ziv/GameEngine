namespace GraphicsRenderer.Components.Shared;

public class Material
{
	public Shader Shader { get; }
	public Texture Texture { get; }

	public Material(Shader shader, Texture texture)
	{
		Shader = shader;
		Texture = texture;
	}

	public void Bind()
	{
		Shader.Bind();
		Texture.Bind();
	}

	public void Unbind()
	{
		Shader.Unbind();
		Texture.Unbind();
	}
}