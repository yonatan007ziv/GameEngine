using GraphicsRenderer.Services.Interfaces.InputProviders;
using OpenTK.Mathematics;

namespace GraphicsRenderer.Components.Interfaces;

internal interface ICamera
{
	int Width { get; set; }
	int Height { get; set; }

	// TEMP: OPENTK PROPRIETARY
	Matrix4 ViewMatrix { get; }
	Matrix4 ProjectionMatrix { get; }

	void Update(IInputProvider inputProvider, float deltaTime);
}