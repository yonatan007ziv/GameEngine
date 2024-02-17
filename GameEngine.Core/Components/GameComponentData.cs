namespace GameEngine.Core.Components;

public readonly struct GameComponentData
{
	public int Id { get; }
	public int ParentId { get; }

	public GameComponentData(int id, int parentId)
	{
		Id = id;
		ParentId = parentId;
	}
}