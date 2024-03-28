namespace GameEngine.Core.Components;

public readonly struct MeshData
{
    public string Model { get; }
    public string Material { get; }

    public MeshData(string model, string material)
    {
        Model = model;
        Material = material;
    }
}