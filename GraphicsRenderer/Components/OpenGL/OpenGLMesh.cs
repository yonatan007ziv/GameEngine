using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GraphicsRenderer.Components.OpenGL;

internal class OpenGLMesh : IMesh
{
	private readonly Model3DData modelData;
	private readonly IShaderManager shaderManager;

	public OpenGLMesh(Model3DData modelData, IShaderManager shaderManager)
	{
		this.modelData = modelData;
		this.shaderManager = shaderManager;
	}

	public void Render(Transform transform, ICamera camera, IShaderProgram shader)
	{
		Matrix4 translationBack = Matrix4.CreateTranslation(transform.Position);

		Matrix4 translationToOrigin = Matrix4.CreateTranslation(-transform.Position);

		Matrix4 scaleMatrix = Matrix4.CreateScale(transform.Scale);

		Matrix4 rotationMatrix =
			Matrix4.CreateRotationX(transform.Rotation.X)
			* Matrix4.CreateRotationY(transform.Rotation.Y)
			* Matrix4.CreateRotationZ(transform.Rotation.Z);

		Matrix4 modelMatrix = translationBack * (translationToOrigin * rotationMatrix * scaleMatrix) * translationBack;
		Matrix4 view = camera.ViewMatrix;
		Matrix4 projection = camera.ProjectionMatrix;

		shaderManager.BindShader(shaderManager.GetShader("Textured"));

		int modelLoc = GL.GetUniformLocation(shaderManager.ActiveShader.Id, "model");
		GL.UniformMatrix4(modelLoc, true, ref modelMatrix);

		int viewLoc = GL.GetUniformLocation(shaderManager.ActiveShader.Id, "view");
		GL.UniformMatrix4(viewLoc, true, ref view);

		int projectionLoc = GL.GetUniformLocation(shaderManager.ActiveShader.Id, "projection");
		GL.UniformMatrix4(projectionLoc, true, ref projection);

		modelData.VertexArray.Bind();
		GL.DrawElements(PrimitiveType.Triangles, modelData.IndicesCount, DrawElementsType.UnsignedInt, 0);
		modelData.VertexArray.Unbind();
	}
}