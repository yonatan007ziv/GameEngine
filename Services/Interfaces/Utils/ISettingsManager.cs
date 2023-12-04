using OpenGLRenderer.Models;

namespace OpenGLRenderer.Services.Interfaces.Utils;

internal interface ISettingsManager
{
	SettingsModel LoadSettings();
}