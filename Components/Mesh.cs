namespace OpenGLRenderer.Components;

internal abstract class Mesh : IDisposable
{
	private readonly GameObject parent;

	public Mesh(GameObject parent)
    {
		this.parent = parent;
		parent.Meshes.Add(this);
	}

	public abstract void Draw();
	public abstract void Dispose();
}