using OpenGLRenderer.Models;
using OpenGLRenderer.Services.Interfaces.Utils.Managers;

namespace OpenGLRenderer.Services.Implementations.Utils;

internal class MockSettingsManager : ISettingsManager
{
	public SettingsModel LoadSettings()
	{
		return new SettingsModel()
		{
			ScreenDimensions = new OpenTK.Mathematics.Vector2i(1000, 1000),
			VSync = true
		};
	}
}