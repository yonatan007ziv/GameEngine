using GraphicsRenderer.Components.Extensions;
using GraphicsRenderer.Components.Interfaces;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Components.Shared.Data;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GraphicsRenderer.Components.OpenGL;

public class OpenGLMeshRenderer : IMeshRenderer
{
	public ModelData Model { get; set; }
	public Material Material { get; set; }

	private Matrix4 modelMatrix;
	private Matrix4 view;
	private Matrix4 projection;

	public OpenGLMeshRenderer(ModelData modelData, Material material)
	{
		Model = modelData;
		Material = material;
	}

	public void Render(ICamera camera)
	{
		view = camera.ViewMatrix.ToOpenTK();
		projection = camera.ProjectionMatrix.ToOpenTK();

		Material.Bind();

		Material.Shader.SetMatrix4Uniform(modelMatrix, "model");
		Material.Shader.SetMatrix4Uniform(view, "view");
		Material.Shader.SetMatrix4Uniform(projection, "projection");

		Model.VertexArray.Bind();
		GL.DrawElements(PrimitiveType.Triangles, Model.IndicesCount, DrawElementsType.UnsignedInt, 0);
		Model.VertexArray.Unbind();
		Material.Unbind();
	}

	public void Update(Transform transform)
	{
		Vector3 openTKPosition = transform.Position.ToOpenTK();

		Matrix4 translationBack = Matrix4.CreateTranslation(openTKPosition);
		Matrix4 translationToOrigin = Matrix4.CreateTranslation(-openTKPosition);

		Matrix4 scaleMatrix = Matrix4.CreateScale(transform.Scale.ToOpenTK());

		Matrix4 rotationMatrix =
			Matrix4.CreateRotationX(transform.Rotation.X)
			* Matrix4.CreateRotationY(transform.Rotation.Y)
			* Matrix4.CreateRotationZ(transform.Rotation.Z);

		modelMatrix = translationBack * (translationToOrigin * rotationMatrix * scaleMatrix) * translationBack;
	}
}