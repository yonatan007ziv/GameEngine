namespace GraphicsEngine.Services.Interfaces;

internal interface IBufferSpecificDeletor
{
    void DeleteBuffer(int id);
    void DeleteVertexArrayBuffer(int id);
    void DeleteTextureBuffer(int id);
}