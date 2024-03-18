using InputEngine.Services.Interfaces;
using System.Runtime.InteropServices;

namespace InputEngine.Services.Implementations.Windows;

internal class ClipboardManagerWindows : IClipboardManager
{
	[DllImport("user32.dll")]
	static extern bool OpenClipboard(IntPtr hWndNewOwner);

	[DllImport("user32.dll")]
	static extern bool CloseClipboard();

	[DllImport("user32.dll")]
	static extern IntPtr GetClipboardData(uint uFormat);

	[DllImport("user32.dll")]
	static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

	[DllImport("user32.dll")]
	static extern bool EmptyClipboard();

	[DllImport("user32.dll")]
	static extern bool IsClipboardFormatAvailable(uint format);

	const uint CF_TEXT = 1;  // Plain text format

	public string GetClipboardText()
	{
		string? text = null;
		if (OpenClipboard(IntPtr.Zero))
		{
			try
			{
				if (IsClipboardFormatAvailable(CF_TEXT))
				{
					IntPtr hClipboardData = GetClipboardData(CF_TEXT);
					if (hClipboardData != IntPtr.Zero)
					{
						IntPtr lpClipboardData = Marshal.StringToHGlobalAnsi(Marshal.PtrToStringAnsi(hClipboardData));
						text = Marshal.PtrToStringAnsi(lpClipboardData);
						Marshal.FreeHGlobal(lpClipboardData);
					}
				}
			}
			finally
			{
				CloseClipboard();
			}
		}

		return text ?? "";
	}

	public void SetClipboardText(string text)
	{
		if (OpenClipboard(IntPtr.Zero))
		{
			try
			{
				EmptyClipboard();
				IntPtr hGlobalMem = Marshal.StringToHGlobalAnsi(text);
				if (SetClipboardData(CF_TEXT, hGlobalMem) == IntPtr.Zero)
					Marshal.FreeHGlobal(hGlobalMem);
			}
			finally
			{
				CloseClipboard();
			}
		}
	}
}