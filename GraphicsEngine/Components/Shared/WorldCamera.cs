using GameEngine.Core.Components;
using GameEngine.Extensions;
using System.Numerics;

namespace GraphicsEngine.Components.Shared;

internal class WorldCamera : Camera
{
    public int Id { get; }
    public int ParentId { get; }

    public Transform Transform { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public ViewPort ViewPort { get; private set; }

    public WorldCamera(int id, int parentId, Transform transform, int width, int height, ViewPort viewPort)
    {
        Id = id;
        ParentId = parentId;
        Transform = transform;

        Width = width;
        Height = height;
        ViewPort = viewPort;

        Update();
    }

    public void Update()
    {
        ViewMatrix = Matrix4x4.CreateLookAt(Transform.Position, Transform.Position + Transform.LocalFront, Transform.LocalUp) * Matrix4x4.CreateScale(-1, 1, 1);
        ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegToRad(FOV), Width / Height, Near, Far);
    }
}