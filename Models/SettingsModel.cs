
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace OpenGLRenderer.Models;

internal struct SettingsModel
{
	public Vector2i ScreenDimensions { get; set; }
	public bool VSync { get; internal set; }
}