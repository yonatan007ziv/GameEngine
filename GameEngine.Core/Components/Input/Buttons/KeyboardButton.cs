namespace GameEngine.Core.Components.Input.Buttons;

public enum KeyboardButton
{
	None,

	Zero = 48, One, Two, Three, Four, Five, Six, Seven, Eight, Nine,
	A = 65, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
	F1 = 101, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,

	RightArrow = 120, LeftArrow, UpArrow, DownArrow,

	// Special characters
	Space,
	Period,
	Comma,
	Tab,
	Backtick,
	Semicolon,
	Equal,
	Minus,
	Apostrophe,
	Slash,
	Backslash,
	LeftSquareBracket,
	RightSquareBracket,

	// Special insertions
	Enter,
	Escape,
	Backspace,
	Delete,


	// Control buttons
	CapsLock,
	LShift,
	LCtrl,
	LAlt,
	RShift,
	RCtrl,
	RAlt,
}