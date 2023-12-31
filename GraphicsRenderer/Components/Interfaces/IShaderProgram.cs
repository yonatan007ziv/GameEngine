using OpenTK.Mathematics;

namespace GraphicsRenderer.Components.Interfaces;

internal interface IShaderProgram : IDisposable
{
	public int Id { get; }
	void Bind();
	void Unbind();
	void SetMatrix4Uniform(ref Matrix4 value, string uniformName);
}