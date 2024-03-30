using GameEngine.Components.ScriptableObjects;
using GameEngine.Components.UIComponents;
using GameEngine.Core.Components.Input.Buttons;

namespace SampleFont;

internal class TestingMovingElement : ScriptableUIObject
{
	private readonly UIButton aButton;
	private readonly UILabel bLabel;
	private readonly UILabel cLabel;
	private readonly UILabel deLabel;

	private int d, e;

	public string Name { get => aButton.Text; set => aButton.Text = value; }
	public string HostName { get => aButton.Text; set => aButton.Text = value; }
	public int CurrentPlayerCount { get => d; set { d = value; UpdateLabel(); } }
	public int MaxPlayerCount { get => e; set { e = value; UpdateLabel(); } }

	public TestingMovingElement(string a, string b, string c, int d, int e)
	{
		aButton = new UIButton("Red.mat");
		bLabel = new UILabel() { Text = b };
		cLabel = new UILabel() { Text = c };
		deLabel = new UILabel();

		aButton.Text = "Click me\nAnd check the console";
		aButton.OnFullClicked += () => Console.WriteLine("Pressed the moving button");

		aButton.Transform.Position = new System.Numerics.Vector3(0.5f, 0.75f, 0);
		aButton.Transform.Scale = new System.Numerics.Vector3(0.25f, 0.25f, 0);

		bLabel.Transform.Position = new System.Numerics.Vector3(-0.5f, 0.55f, 0);
		bLabel.Transform.Scale = new System.Numerics.Vector3(0.25f, 0.25f, 0);
		bLabel.Meshes.Add(new GameEngine.Core.Components.MeshData("UIRect.obj", "Blue.mat"));

		cLabel.Transform.Scale = new System.Numerics.Vector3(0.25f, 0.25f, 0);
		cLabel.Meshes.Add(new GameEngine.Core.Components.MeshData("UIRect.obj", "missing texture and stuff"));

		deLabel.Meshes.Add(new GameEngine.Core.Components.MeshData("UIRect.obj", "Red.mat"));
		deLabel.Transform.Scale /= 4;
		deLabel.Transform.Position = new System.Numerics.Vector3(-0.5f, -0.5f, 0);

		this.d = d;
		this.e = e;
		UpdateLabel();

		Meshes.Add(new GameEngine.Core.Components.MeshData("UIRect.obj", "Green.mat"));

		Children.Add(aButton);
		Children.Add(bLabel);
		Children.Add(cLabel);
		Children.Add(deLabel);
	}

	public void UpdateLabel()
	{
		deLabel.Text = $"({d}/{e})";
	}

	public override void Update(float deltaTime)
	{
		if (GetKeyboardButtonPressed(KeyboardButton.A))
			Transform.Position -= System.Numerics.Vector3.UnitX * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.D))
			Transform.Position += System.Numerics.Vector3.UnitX * deltaTime / 5;

		if (GetKeyboardButtonPressed(KeyboardButton.S))
			Transform.Position -= System.Numerics.Vector3.UnitY * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.W))
			Transform.Position += System.Numerics.Vector3.UnitY * deltaTime / 5;

		if (GetKeyboardButtonPressed(KeyboardButton.Q))
			Transform.Scale -= System.Numerics.Vector3.One * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.E))
			Transform.Scale += System.Numerics.Vector3.One * deltaTime / 5;

		if (GetKeyboardButtonPressed(KeyboardButton.F))
			Transform.Scale -= System.Numerics.Vector3.UnitX * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.R))
			Transform.Scale += System.Numerics.Vector3.UnitX * deltaTime / 5;

		if (GetKeyboardButtonPressed(KeyboardButton.G))
			Transform.Scale -= System.Numerics.Vector3.UnitY * deltaTime / 5;
		if (GetKeyboardButtonPressed(KeyboardButton.T))
			Transform.Scale += System.Numerics.Vector3.UnitY * deltaTime / 5;
	}
}