namespace GraphicsEngine.Components.Shared;

internal class Material
{
    private bool _tile;
    public bool Tile { get => _tile; set { Texture.Tile(value); _tile = value; } }
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