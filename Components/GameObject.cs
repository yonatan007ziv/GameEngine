using OpenTK.Mathematics;

namespace OpenGLRenderer.Components;

internal class GameObject : IDisposable
{
	public Vector3 Position { get; set; }
	public List<Mesh> Meshes { get; set; } = new List<Mesh>();

	public GameObject(Vector3 origin)
	{
		Position = origin;
	}

	public void Draw()
	{
		Meshes.ForEach(m => m.Draw());
	}


	/*
	public Matrix4 RotateBy(???)
	{
		
	}
	*/

	public void Delete()
		=> Dispose();
	public void Dispose()
	{
		Meshes.ForEach(m => m.Dispose());
	}
}