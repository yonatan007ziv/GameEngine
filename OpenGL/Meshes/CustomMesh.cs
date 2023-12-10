using OpenGLRenderer.Components;
using OpenGLRenderer.Models;
using OpenGLRenderer.Services.Implementations.Utils.Managers;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGLRenderer.OpenGL.Meshes;

internal class CustomMesh : Mesh
{
	private readonly ModelData modelData;
	private readonly IShaderManager shaderManager;

	public CustomMesh(ModelData modelData, IShaderManager shaderManager)
	{
		this.modelData = modelData;
		this.shaderManager = shaderManager;
	}

	public override void Render(Transform transform)
	{
		Matrix4 translationBack = Matrix4.CreateTranslation(transform.Position);
		Matrix4 translationToOrigin = Matrix4.CreateTranslation(-transform.Position);
		Matrix4 scaleMatrix = Matrix4.CreateScale(transform.Scale);
		Matrix4 rotationMatrix =
			Matrix4.CreateRotationX(transform.Rotation.X)
			* Matrix4.CreateRotationY(transform.Rotation.Y)
			* Matrix4.CreateRotationZ(transform.Rotation.Z);
		Matrix4 modelMatrix = translationBack * (translationToOrigin * rotationMatrix * scaleMatrix) * translationBack;

		GL.UniformMatrix4(shaderManager.ActiveShader.Id, true, ref modelMatrix);

		modelData.VertexArray.Bind();
		GL.DrawElements(PrimitiveType.Triangles, modelData.IndicesCount, DrawElementsType.UnsignedInt, 0);
		modelData.VertexArray.Unbind();
	}
}