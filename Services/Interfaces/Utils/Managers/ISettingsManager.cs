using OpenGLRenderer.Models;

namespace OpenGLRenderer.Services.Interfaces.Utils.Managers;

internal interface ISettingsManager
{
	SettingsModel LoadSettings();
}