using GameEngine.Core.Components;
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

	public void Render(Camera camera)
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

	public void Update(Transform transform, Transform? relativeTransform = null)
	{
		relativeTransform ??= Transform.Identity;

		Matrix4x4 relativeRotationMatrix =
			Matrix4x4.CreateRotationX(MathHelper.DegToRad(transform.Rotation.X + relativeTransform.Rotation.X))
			* Matrix4x4.CreateRotationY(MathHelper.DegToRad(transform.Rotation.Y + relativeTransform.Rotation.Y + 180))
			* Matrix4x4.CreateRotationZ(MathHelper.DegToRad(transform.Rotation.Z + relativeTransform.Rotation.Z));

		modelMatrix = relativeRotationMatrix * Matrix4x4.CreateScale(transform.Scale * relativeTransform.Scale) * Matrix4x4.CreateTranslation(transform.Position * relativeTransform.Scale + relativeTransform.Position);
	}
}