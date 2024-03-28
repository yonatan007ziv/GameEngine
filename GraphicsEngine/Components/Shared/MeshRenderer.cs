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

    public void Update(Transform transform)
    {
        Matrix4x4 rotationMatrix =
            Matrix4x4.CreateRotationX(MathHelper.DegToRad(transform.Rotation.X))
            * Matrix4x4.CreateRotationY(MathHelper.DegToRad(transform.Rotation.Y + 180))
            * Matrix4x4.CreateRotationZ(MathHelper.DegToRad(transform.Rotation.Z));

        modelMatrix = rotationMatrix * Matrix4x4.CreateScale(transform.Scale) * Matrix4x4.CreateTranslation(transform.Position);
    }
}