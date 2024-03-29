using System.Numerics;

namespace GraphicsEngine.Components.Interfaces;

public interface IShaderProgram : IDisposable
{
	public int Id { get; }
	void Bind();
	void Unbind();
	void SetMatrix4Uniform(Matrix4x4 value, string uniformName);
}