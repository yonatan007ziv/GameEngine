using OpenGLRenderer.Components;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLRenderer.OpenGL.Meshes;

internal class PrimitiveTriangle : Mesh
{
	private readonly VertexArray va;
	private readonly VertexBuffer vb;
	private readonly IndexBuffer ib;
	private readonly ShaderProgram shaderProgram;

	public PrimitiveTriangle(GameObject parent, Vector3 v1, Vector3 v2, Vector3 v3, ShaderProgram shaderProgram)
		: base(parent)
	{
		vb = new VertexBuffer();
		vb.WriteBuffer<float>(new float[]
		{
			v1.X + parent.Position.X, v1.Y + parent.Position.Y, v1.Z + parent.Position.Z,
			v2.X + parent.Position.X, v2.Y + parent.Position.Y, v2.Z + parent.Position.Z,
			v3.X + parent.Position.X, v3.Y + parent.Position.Y, v3.Z + parent.Position.Z,
		}, BufferUsageHint.StaticDraw);

		ib = new IndexBuffer();
		ib.WriteBuffer(new uint[] { 0, 1, 2 });

		va = new VertexArray(vb, ib);
		this.shaderProgram = shaderProgram;
	}

	public override void Draw()
	{
		va.Bind();
		shaderProgram.Bind();

		GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
	}

	public void Delete()
		=> Dispose();

	public override void Dispose()
	{
		vb.Dispose();
		va.Dispose();
		shaderProgram.Dispose();
	}
}