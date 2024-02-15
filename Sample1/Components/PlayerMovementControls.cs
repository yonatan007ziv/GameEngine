namespace Sample1.Components;

internal struct PlayerMovementControls
{
	public AxesSet AxesSet;
	public string Jump;
	public string Pause;

	public PlayerMovementControls(AxesSet axesSet, string jump, string pause)
	{
		AxesSet = axesSet;
		Jump = jump;
		Pause = pause;
	}
}