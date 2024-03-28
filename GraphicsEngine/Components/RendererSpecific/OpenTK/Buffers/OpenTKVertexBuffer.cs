using GraphicsEngine.Components.Interfaces.Buffers;
using OpenTK.Graphics.OpenGL4;

namespace GraphicsEngine.Components.RendererSpecific.OpenTK.Buffers;

internal class OpenTKVertexBuffer : OpenTKBuffer, IVertexBuffer
{

    public OpenTKVertexBuffer()
        : base(BufferTarget.ArrayBuffer)
    {

    }

    public void WriteData<T>(T[] data) where T : struct
        => WriteBuffer(data, BufferUsageHint.StaticDraw);
}