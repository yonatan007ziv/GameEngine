
using System.Numerics;

namespace GraphicsRenderer.Components.Shared.Data;

internal struct SettingsData
{
	public Vector2 ScreenDimensions { get; set; }
	public bool VSync { get; internal set; }
}