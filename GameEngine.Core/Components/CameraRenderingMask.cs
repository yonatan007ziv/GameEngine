namespace GameEngine.Core.Components;

public class CameraRenderingMask
{

    private readonly ICollection<string> mask = new List<string>();

	public bool MaskContains(string maskElement) => mask.Contains(maskElement);
	public bool AddMask(string maskElement) 
	{
		// Check if not empty or default
		if (maskElement != string.Empty && !MaskContains(maskElement))
		{
			mask.Add(maskElement);
			return true;
		}

		return false;
    }
}