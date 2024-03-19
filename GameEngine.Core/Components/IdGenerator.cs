namespace GameEngine.Core.Components;

internal static class IdGenerator
{
	private static int currentId = 0;

	public static int GenerateNext()
		=> currentId++;
}