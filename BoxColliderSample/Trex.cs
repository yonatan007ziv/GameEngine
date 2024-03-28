using GameEngine.Components.ScriptableObjects;
using GameEngine.Core.Components;
using GameEngine.Core.Components.Input.Buttons;

namespace BoxColliderSample;

internal class Trex : ScriptableWorldObject
{
    public Trex()
    {
        Tag = "Trex";
        Meshes.Add(new MeshData("Trex.obj", "Trex.mat"));
    }

    public override void Update(float deltaTime)
    {
        if (GetKeyboardButtonDown(KeyboardButton.Delete))
            Visible = !Visible;
    }

    ~Trex()
    {
        Console.WriteLine($"Disposed ScriptableWorldObject: {Tag}");
    }
}