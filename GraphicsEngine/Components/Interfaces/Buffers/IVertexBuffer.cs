namespace GraphicsEngine.Components.Interfaces.Buffers;

internal interface IVertexBuffer : IBuffer
{
    void Bind();
    void Unbind();
    void WriteData<T>(T[] data) where T : struct;
}