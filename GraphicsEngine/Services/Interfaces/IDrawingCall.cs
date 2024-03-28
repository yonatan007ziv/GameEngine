using GraphicsEngine.Components.Shared.Data;

namespace GraphicsEngine.Services.Interfaces;

internal interface IDrawingCall
{
    void DrawCall(ModelData modelData);
}