namespace GraphicsEngine.Services.Interfaces;

internal interface IBufferDeletor
{
	void DeleteBuffer(int id);
	void DeleteVertexArrayBuffer(int id);
	void DeleteTextureBuffer(int id);
}