using System.Numerics;

namespace GraphicsRenderer.Components.Interfaces;

public interface ICamera : IComponent
{
	float Width { get; set; }
	float Height { get; set; }
	int SensitivitySpeed { get; }
	Matrix4x4 ViewMatrix { get; }
	Matrix4x4 ProjectionMatrix { get; }

	void Update(float deltaTime);
}