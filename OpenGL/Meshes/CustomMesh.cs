using OpenGLRenderer.Components;
using OpenGLRenderer.Models;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLRenderer.OpenGL.Meshes;

internal class CustomMesh : Mesh
{
	private readonly ModelData modelData;
	private readonly ShaderProgram shaderProgram;

	public CustomMesh(GameObject Parent, ModelData modelData, ShaderProgram shaderProgram)
		: base(Parent)
	{
		this.modelData = modelData;
		this.shaderProgram = shaderProgram;
	}

	public override void Draw()
	{
		modelData.VertexArray.Bind();
		shaderProgram.Bind();

		GL.DrawArrays(PrimitiveType.Triangles, 0, 3); // temp 5 hard coded
	}

	public override void Dispose()
	{
		throw new NotImplementedException();
	}
}