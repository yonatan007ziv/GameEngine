using GraphicsRenderer.Components.Shared.Data;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;
using System.Numerics;

namespace GraphicsRenderer.Services.Implementations.Shared.Mocks;

internal class MockSettingsManager : ISettingsManager
{
	public SettingsData LoadSettings()
	{
		return new SettingsData()
		{
			ScreenDimensions = new Vector2(1920 / 2, 1080 / 2),
			VSync = true
		};
	}

	public void SaveSettings(SettingsData settings)
	{

	}
}