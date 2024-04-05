using GameEngine.Components.ScriptableObjects;
using GameEngine.Components.UIComponents;
using GameEngine.Core.Components.Input.Buttons;
using System.Drawing;
using System.Numerics;

namespace SampleFont;

internal class TestingRecursiveElement : ScriptableUIObject
{
	public TestingRecursiveElement(int times)
	{
		Meshes.Add(new GameEngine.Core.Components.MeshData("UIRect.obj", ""));

		Color color = Color.Purple;
		if (times == 4)
			color = Color.Red;
		else if (times == 3)
			color = Color.Green;
		else if (times == 2)
			color = Color.Blue;
		else if (times == 1)
			color = Color.Black;
		else if (times == 0)
			color = Color.White;


		UIButton button = new UIButton();
		button.Text = "Recursion";
		button.TextColor = color;
		button.Transform.Scale /= 2;
		button.Transform.Position = new Vector3(0.5f, 0, 0);
		Children.Add(button);

		if (times >= 0)
		{
			TestingRecursiveElement recursiveElement = new TestingRecursiveElement(times - 1);
			recursiveElement.Transform.Scale /= 2;
			recursiveElement.Transform.Position = new Vector3(-0.5f, 0, 0);
			Children.Add(recursiveElement);
		}
	}

	public override void Update(float deltaTime)
	{
		if (GetKeyboardButtonPressed(KeyboardButton.A))
			Transform.Position -= Vector3.UnitX * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.D))
			Transform.Position += Vector3.UnitX * deltaTime / 5;

		if (GetKeyboardButtonPressed(KeyboardButton.S))
			Transform.Position -= Vector3.UnitY * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.W))
			Transform.Position += Vector3.UnitY * deltaTime / 5;

		if (GetKeyboardButtonPressed(KeyboardButton.Q))
			Transform.Scale -= Vector3.One * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.E))
			Transform.Scale += Vector3.One * deltaTime / 5;

		if (GetKeyboardButtonPressed(KeyboardButton.F))
			Transform.Scale -= Vector3.UnitX * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.R))
			Transform.Scale += Vector3.UnitX * deltaTime / 5;

		if (GetKeyboardButtonPressed(KeyboardButton.G))
			Transform.Scale -= Vector3.UnitY * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.T))
			Transform.Scale += Vector3.UnitY * deltaTime / 5;
	}
}