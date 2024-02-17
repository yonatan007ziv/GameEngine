using GameEngine.Components.Objects;
using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components.Input.Buttons;
using Sample1.Scenes;

namespace Sample1.Components;

internal class SceneSwitcher : ScriptableWorldComponent
{
	private readonly bool singlePlayer;

	public SceneSwitcher(WorldObject parent, bool singlePlayer)
		: base(parent)
	{
		this.singlePlayer = singlePlayer;
	}

	public override void Update(float deltaTime)
	{
		if (GetKeyboardButtonDown(KeyboardButton.Four))
		{
			if (singlePlayer)
				new SingleCameraScene().LoadScene();
			else
				new SplitScreenScene().LoadScene();
		}
	}
}