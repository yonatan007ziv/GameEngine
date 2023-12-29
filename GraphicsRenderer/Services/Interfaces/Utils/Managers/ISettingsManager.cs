using GraphicsRenderer.Components.Shared.Data;

namespace GraphicsRenderer.Services.Interfaces.Utils.Managers;

internal interface ISettingsManager
{
	void SaveSettings(SettingsData settings);
	SettingsData LoadSettings();
}