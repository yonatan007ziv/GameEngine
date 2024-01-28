using GameEngine.Core.Components;
using GraphicsEngine.Components.Extensions;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GraphicsEngine.Components.OpenGL;

internal class OpenGLMeshRenderer : IMeshRenderer
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

	public void Render(Camera camera)
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
		Matrix4 scaleMatrix = Matrix4.CreateScale(transform.Scale.ToOpenTK());

		Matrix4 rotationMatrix =
			Matrix4.CreateRotationX(MathHelper.DegreesToRadians(transform.Rotation.X))
			* Matrix4.CreateRotationY(MathHelper.DegreesToRadians(transform.Rotation.Y))
			* Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(transform.Rotation.Z));

		modelMatrix = rotationMatrix * scaleMatrix * Matrix4.CreateTranslation(openTKPosition);
	}
}