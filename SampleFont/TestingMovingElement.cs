using GameEngine.Components.ScriptableObjects;
using GameEngine.Components.UIComponents;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Objects;
using System.Numerics;

namespace SampleFont;

internal class TestingMovingElement : ScriptableUIObject
{
	public bool EnabledMovement { get; set; }

	private readonly UIButton button;

	public TestingMovingElement(int count)
	{
		button = new UIButton();
		button.Text = "Click me\nAnd check the console";
		button.OnFullClicked += () => Console.WriteLine("Pressed the moving button");
		button.Transform.Scale = new Vector3(0.5f, 0.5f, 1);

		string mat;
		if (count == 1)
			mat = "Wall.mat";
		else if (count == 2)
			mat = "Green.mat";
		else if (count == 3)
			mat = "Blue.mat";
		else
			mat = "Ground.mat";

		Meshes.Add(new GameEngine.Core.Components.MeshData("UIRect.obj", mat));
		button.Meshes.Add(new GameEngine.Core.Components.MeshData("UIRect.obj", "Green.mat"));
		Children.Add(button);

		if (count > 0)
		{
			UIObject obj = new TestingMovingElement(count - 1);
			obj.Transform.Scale /= 5;
			obj.Transform.Position -= Vector3.One * count / (5);
			Children.Add(obj);
		}
	}

	public override void Update(float deltaTime)
	{
		if (!EnabledMovement)
			return;

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