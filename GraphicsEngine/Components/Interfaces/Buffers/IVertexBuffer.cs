namespace GraphicsEngine.Components.Interfaces.Buffers;

internal interface IVertexBuffer
{
	void Bind();
	void Unbind();
	void WriteData<T>(T[] data) where T : struct;
}