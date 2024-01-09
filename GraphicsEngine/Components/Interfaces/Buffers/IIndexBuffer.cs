namespace GraphicsEngine.Components.Interfaces.Buffers;

public interface IIndexBuffer
{
	void Bind();
	void Unbind();
	void WriteData(uint[] data);
}