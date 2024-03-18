namespace InputEngine.Services.Interfaces;

internal interface IClipboardManager
{
	string GetClipboardText();
	void SetClipboardText(string text);
}