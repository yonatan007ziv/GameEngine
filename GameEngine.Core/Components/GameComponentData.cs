namespace GameEngine.Core.Components;

public readonly struct GameComponentData
{
	public int Id { get; }
	public int ParentId { get; }
	public bool UI { get; }

	public GameComponentData(int id, int parentId, bool ui)
	{
		Id = id;
		ParentId = parentId;
		UI = ui;
	}
}