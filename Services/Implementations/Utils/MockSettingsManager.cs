using OpenGLRenderer.Models;
using OpenGLRenderer.Services.Interfaces.Utils;

namespace OpenGLRenderer.Services.Implementations.Utils;

internal class MockSettingsManager : ISettingsManager
{
	public SettingsModel LoadSettings()
	{
		return new SettingsModel()
		{
			ScreenDimensions = new OpenTK.Mathematics.Vector2i(700, 700),
			VSync = true
		};
	}
}