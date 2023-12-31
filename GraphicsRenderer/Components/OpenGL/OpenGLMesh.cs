using GraphicsRenderer.Components.Extensions;
using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Components.Shared.Data;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GraphicsRenderer.Components.OpenGL;

internal class OpenGLMesh : IMesh
{
	private readonly ModelData modelData;

	public OpenGLMesh(ModelData modelData)
	{
		this.modelData = modelData;
	}

	public void Render(Transform transform, ICamera camera, Material material)
	{
		Vector3 openTKPosition = transform.Position.ToOpenTK();

		Matrix4 translationBack = Matrix4.CreateTranslation(openTKPosition);
		Matrix4 translationToOrigin = Matrix4.CreateTranslation(-openTKPosition);

		Matrix4 scaleMatrix = Matrix4.CreateScale(transform.Scale.ToOpenTK());

		Matrix4 rotationMatrix =
			Matrix4.CreateRotationX(transform.Rotation.X)
			* Matrix4.CreateRotationY(transform.Rotation.Y)
			* Matrix4.CreateRotationZ(transform.Rotation.Z);

		Matrix4 modelMatrix = translationBack * (translationToOrigin * rotationMatrix * scaleMatrix) * translationBack;
		Matrix4 view = camera.ViewMatrix;
		Matrix4 projection = camera.ProjectionMatrix;

		material.Bind();

		int modelLoc = GL.GetUniformLocation(material.ShaderProgram.Id, "model");
		GL.UniformMatrix4(modelLoc, true, ref modelMatrix);

		int viewLoc = GL.GetUniformLocation(material.ShaderProgram.Id, "view");
		GL.UniformMatrix4(viewLoc, true, ref view);

		int projectionLoc = GL.GetUniformLocation(material.ShaderProgram.Id, "projection");
		GL.UniformMatrix4(projectionLoc, true, ref projection);

		modelData.VertexArray.Bind();
		GL.DrawElements(PrimitiveType.Triangles, modelData.IndicesCount, DrawElementsType.UnsignedInt, 0);
		modelData.VertexArray.Unbind();
		material.Unbind();
	}
}