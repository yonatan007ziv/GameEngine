using OpenTK.Mathematics;

namespace GraphicsEngine.Components.Interfaces;

public interface IShaderProgram : IDisposable
{
	public int Id { get; }
	void Bind();
	void Unbind();
	void SetMatrix4Uniform(ref Matrix4 value, string uniformName);
}