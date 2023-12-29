namespace GraphicsRenderer.Components.Interfaces;

internal interface IShaderProgram : IDisposable
{
	public int Id { get; }
	void Bind();
	void Unbind();
}