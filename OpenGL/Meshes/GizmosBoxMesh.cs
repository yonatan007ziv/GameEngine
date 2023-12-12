using OpenGLRenderer.Components;
using OpenGLRenderer.Models;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenGLRenderer.OpenGL.Meshes;

internal class GizmosBoxMesh : Mesh
{
	private ModelData modelData;
	private readonly IShaderManager shaderManager;

	public GizmosBoxMesh(Box box, IShaderManager shaderManager)
	{
		this.shaderManager = shaderManager;

		modelData = new ModelData(
			new VertexBuffer(),
			new IndexBuffer(),
			new TextureBuffer(),
			box,
			8);

		float[] positions = box.JoinAll();
		// Mock Texture Coordinates
		float[] newArray = positions.SelectMany((value, index) => index % 3 == 2 ? new[] { value, 0.0f, 1.0f } : new[] { value }).ToArray();
		modelData.VertexBuffer.WriteBuffer(newArray);

		uint[] indexBuffer = new uint[]
		{

			// right
			1, 5,
			5, 6,
			6, 2,
			2, 1,

			// left
			0, 4,
			4, 7,
			7, 3,
			3, 0,

			// front
			0, 1,
			1, 2,
			2, 3,
			3, 0,

			// back
			4, 7,
			7, 6,
			6, 5,
			5, 4,
		};
		modelData.IndexBuffer.WriteBuffer(indexBuffer);
	}

	public override void Render(Transform transform, Camera camera)
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

		shaderManager.BindShader(shaderManager.ShaderBank.GetGizmosShader());

		int modelLoc = GL.GetUniformLocation(shaderManager.ActiveShader.Id, "model");
		GL.UniformMatrix4(modelLoc, true, ref modelMatrix);


		int viewLoc = GL.GetUniformLocation(shaderManager.ActiveShader.Id, "view");
		GL.UniformMatrix4(viewLoc, true, ref view);

		int projectionLoc = GL.GetUniformLocation(shaderManager.ActiveShader.Id, "projection");
		GL.UniformMatrix4(projectionLoc, true, ref projection);

		modelData.VertexArray.Bind();
		GL.DrawElements(PrimitiveType.Lines, 32, DrawElementsType.UnsignedInt, 0);
		modelData.VertexArray.Unbind();
	}
}