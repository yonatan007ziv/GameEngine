namespace GraphicsEngine.Components.Interfaces.Buffers;

internal interface IIndexBuffer : IBuffer
{
    void Bind();
    void Unbind();
    void WriteData(uint[] data);
}