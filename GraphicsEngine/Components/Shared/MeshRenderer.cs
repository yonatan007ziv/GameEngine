using GameEngine.Core.Components;
using GameEngine.Core.Components.Objects;
using GameEngine.Extensions;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Interfaces;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal class MeshRenderer
{
	private readonly IDrawingCall drawingCall;

	public ModelData Model { get; set; }
	public Material Material { get; set; }

	private Matrix4x4 modelMatrix;
	private Matrix4x4 view;
	private Matrix4x4 projection;

	public MeshRenderer(IDrawingCall drawingCall, ModelData modelData, Material material)
	{
		this.drawingCall = drawingCall;
		Model = modelData;
		Material = material;
	}

	public void Render(RenderingCamera camera)
	{
		view = camera.ViewMatrix;
		projection = camera.ProjectionMatrix;

		Material.Bind();

		Material.Shader.SetMatrix4Uniform(modelMatrix, "model");
		Material.Shader.SetMatrix4Uniform(view, "view");
		Material.Shader.SetMatrix4Uniform(projection, "projection");

		drawingCall.DrawCall(Model);
		Material.Unbind();
	}

	public void Update(GameObject gameObject)
	{
		(Vector3 position, Vector3 rotation, Vector3 scale)  relativeTransform = gameObject.GetRelativeToAncestorTransform();

		modelMatrix =
			Matrix4x4.CreateRotationX(MathHelper.DegToRad(relativeTransform.rotation.X))
			* Matrix4x4.CreateRotationY(MathHelper.DegToRad(relativeTransform.rotation.Y))
			* Matrix4x4.CreateRotationZ(MathHelper.DegToRad(relativeTransform.rotation.Z))
			* Matrix4x4.CreateScale(relativeTransform.scale) * Matrix4x4.CreateTranslation(relativeTransform.position);
	}
}