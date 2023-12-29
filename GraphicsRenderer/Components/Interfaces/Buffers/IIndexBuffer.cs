namespace GraphicsRenderer.Components.Interfaces.Buffers;

internal interface IIndexBuffer
{
	void Bind();
	void Unbind();
	void WriteData(uint[] data);
}