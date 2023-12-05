
using OpenTK.Mathematics;

namespace OpenGLRenderer.Models;

internal struct SettingsModel
{
	public Vector2i ScreenDimensions { get; set; }
	public bool VSync { get; internal set; }
}