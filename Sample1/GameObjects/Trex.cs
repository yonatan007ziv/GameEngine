using GameEngine.Components;
using GameEngine.Components.Objects.Scriptable;
using GameEngine.Core.Components;

namespace Sample1.GameObjects;

internal class Trex : ScriptableWorldObject
{
	public Trex()
	{
		Meshes.Add(new MeshData("Trex.obj", "Hamama.mat"));
	}

	public override void Update(float deltaTime)
	{
		if (GetKeyboardButtonDown(GameEngine.Core.Components.Input.Buttons.KeyboardButton.Q))
			Scene.LoadedScene.worldObjects.Remove(this);
	}
}