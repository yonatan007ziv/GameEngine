using GameEngine.Components;
using GameEngine.Components.UIComponents;
using GameEngine.Core.Components;
using System.Numerics;

namespace SampleFont;

internal class FontTestingScene : Scene
{
	public FontTestingScene()
	{
		Vector3 buttonScale = new Vector3(0.25f, 0.25f, 0);

		UICameras.Add((new UICamera(), new ViewPort(0.5f, 0.5f, 1, 1)));

		UIButton _switchToLobbySelection = new UIButton();
		_switchToLobbySelection.Text = "Lobby Selection";
		_switchToLobbySelection.TextColor = System.Drawing.Color.White;
		_switchToLobbySelection.Transform.Scale = buttonScale;
		_switchToLobbySelection.Transform.Position = new Vector3(-0.5f, 0, 0);
		UIObjects.Add(_switchToLobbySelection);

		UIButton _switchToGameSettings = new UIButton();
		_switchToGameSettings.Text = "Game Settings";
		_switchToGameSettings.TextColor = System.Drawing.Color.White;
		_switchToGameSettings.Transform.Scale = buttonScale;
		_switchToGameSettings.Transform.Position = new Vector3(0.5f, 0, 0);
		UIObjects.Add(_switchToGameSettings);

		// UICameras.Add((new UICamera(), new GameEngine.Core.Components.ViewPort(0.5f, 0.5f, 1, 1)));

		// TestingRecursiveElement recursiveElement = new TestingRecursiveElement(0);
		// recursiveElement.Transform.Scale /= 2;
		// UIObjects.Add(recursiveElement);
	}
}