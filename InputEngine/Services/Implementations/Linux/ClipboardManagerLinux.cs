using InputEngine.Services.Interfaces;

namespace InputEngine.Services.Implementations.Linux;

internal class ClipboardManagerLinux : IClipboardManager
{
    public string GetClipboardText()
    {
        throw new NotImplementedException();
    }

    public void SetClipboardText(string text)
    {
        throw new NotImplementedException();
    }
}