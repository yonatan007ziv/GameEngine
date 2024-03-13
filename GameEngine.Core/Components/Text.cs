using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameEngine.Core.Components;

public class Text : INotifyPropertyChanged
{
	private string _text;
	private string _fontName;
	private int _fontSize;

	public string Txt { get => _text; set { _text = value; OnPropertyChanged(); } }
	public string FontName { get => _fontName; set { _fontName = value; OnPropertyChanged(); } }
	public int FontSize { get => _fontSize; set { _fontSize = value; OnPropertyChanged(); } }

	public Text(string text, string fontName, int fontSize)
	{
		_text = text;
		_fontName = fontName;
		_fontSize = fontSize;
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}