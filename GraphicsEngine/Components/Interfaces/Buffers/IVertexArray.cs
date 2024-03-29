namespace GraphicsEngine.Components.Interfaces.Buffers;

internal interface IVertexArray : IBuffer
{
	void Bind();
	void Unbind();
}