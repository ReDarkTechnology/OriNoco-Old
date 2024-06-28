using System;
using System.Collections.Generic;

namespace OriNoco
{
	public enum MouseCode : uint
	{
		Right = 2u,
		Left = 1u,
		X2 = 5u,
		X1 = 4u,
		Middle = 3u,
	}
	public enum KeyCode
	{
		Unknown,
		Return = 13,
		Escape = 27,
		Backspace = 8,
		Tab,
		Space = 32,
		Exclaim,
		Quotedbl,
		Hash,
		Percent = 37,
		Dollar = 36,
		Ampersand = 38,
		Quote,
		Leftparen,
		Rightparen,
		Asterisk,
		Plus,
		Comma,
		Minus,
		Period,
		Slash,
		Alpha_0,
		Alpha_1,
		Alpha_2,
		Alpha_3,
		Alpha_4,
		Alpha_5,
		Alpha_6,
		Alpha_7,
		Alpha_8,
		Alpha_9,
		Colon,
		Semicolon,
		Less,
		Equals,
		Greater,
		Question,
		At,
		Leftbracket = 91,
		Backslash,
		Rightbracket,
		Caret,
		Underscore,
		Backquote,
		A,
		B,
		C,
		D,
		E,
		F,
		G,
		H,
		I,
		J,
		K,
		L,
		M,
		N,
		O,
		P,
		Q,
		R,
		S,
		T,
		U,
		V,
		W,
		X,
		Y,
		Z,
		Capslock = 1073741881,
		F1,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		F11,
		F12,
		Printscreen,
		Scrolllock,
		Pause,
		Insert,
		Home,
		Pageup,
		Delete = 127,
		End = 1073741901,
		Pagedown,
		Right,
		Left,
		Down,
		Up,
		Numlockclear,
		Kp_divide,
		Kp_multiply,
		Kp_minus,
		Kp_plus,
		Kp_enter,
		Kp_1,
		Kp_2,
		Kp_3,
		Kp_4,
		Kp_5,
		Kp_6,
		Kp_7,
		Kp_8,
		Kp_9,
		Kp_0,
		Kp_period,
		Application = 1073741925,
		Power,
		Kp_equals,
		F13,
		F14,
		F15,
		F16,
		F17,
		F18,
		F19,
		F20,
		F21,
		F22,
		F23,
		F24,
		Execute,
		Help,
		Menu,
		Select,
		Stop,
		Again,
		Undo,
		Cut,
		Copy,
		Paste,
		Find,
		Mute,
		Volumeup,
		Volumedown,
		Kp_comma = 1073741957,
		Kp_equalsas400,
		Alterase = 1073741977,
		Sysreq,
		Cancel,
		Clear,
		Prior,
		Return2,
		Separator,
		Out,
		Oper,
		Clearagain,
		Crsel,
		Exsel,
		Kp_00 = 1073742000,
		Kp_000,
		Thousandsseparator,
		Decimalseparator,
		Currencyunit,
		Currencysubunit,
		Kp_leftparen,
		Kp_rightparen,
		Kp_leftbrace,
		Kp_rightbrace,
		Kp_tab,
		Kp_backspace,
		Kp_a,
		Kp_b,
		Kp_c,
		Kp_d,
		Kp_e,
		Kp_f,
		Kp_xor,
		Kp_power,
		Kp_percent,
		Kp_less,
		Kp_greater,
		Kp_ampersand,
		Kp_dblampersand,
		Kp_verticalbar,
		Kp_dblverticalbar,
		Kp_colon,
		Kp_hash,
		Kp_space,
		Kp_at,
		Kp_exclam,
		Kp_memstore,
		Kp_memrecall,
		Kp_memclear,
		Kp_memadd,
		Kp_memsubtract,
		Kp_memmultiply,
		Kp_memdivide,
		Kp_plusminus,
		Kp_clear,
		Kp_clearentry,
		Kp_binary,
		Kp_octal,
		Kp_decimal,
		Kp_hexadecimal,
		Lctrl = 1073742048,
		Lshift,
		Lalt,
		Lgui,
		Rctrl,
		Rshift,
		Ralt,
		Rgui,
		Mode = 1073742081,
		Audionext,
		Audioprev,
		Audiostop,
		Audioplay,
		Audiomute,
		Mediaselect,
		Www,
		Mail,
		Calculator,
		Computer,
		Ac_search,
		Ac_home,
		Ac_back,
		Ac_forward,
		Ac_stop,
		Ac_refresh,
		Ac_bookmarks,
		Brightnessdown,
		Brightnessup,
		Displayswitch,
		Kbdillumtoggle,
		Kbdillumdown,
		Kbdillumup,
		Eject,
		Sleep,
		Mouse0,
		Mouse1,
		Mouse2
	}
	
	public static class EnumKit
	{
		public static Dictionary<string, string> dict = new Dictionary<string, string>();
		public static void Prepare()
		{
			dict.Add("Return", "\n");
			dict.Add("Backspace", "\b");
			dict.Add("Tab", "\t");
			dict.Add("Space", " ");
			dict.Add("Exclaim", "!");
			dict.Add("Quotedbl", "\"");
			dict.Add("Hash", "#");
			dict.Add("Percent", "%");
			dict.Add("Dollar", "$");
			dict.Add("Ampersand", "&");
			dict.Add("Quote", "'");
			dict.Add("Asterisk", "*");
			dict.Add("Plus", "+");
			dict.Add("Minus", "-");
			dict.Add("Period", ".");
			dict.Add("Slash", "/");
			dict.Add("Alpha_0", "0");
			dict.Add("Alpha_1", "1");
			dict.Add("Alpha_2", "2");
			dict.Add("Alpha_3", "3");
			dict.Add("Alpha_4", "4");
			dict.Add("Alpha_5", "5");
			dict.Add("Alpha_6", "6");
			dict.Add("Alpha_7", "7");
			dict.Add("Alpha_8", "8");
			dict.Add("Alpha_9", "9");
			dict.Add("Colon", ":");
			dict.Add("Semicolon", ";");
			dict.Add("Less", "<");
			dict.Add("Equals", "=");
			dict.Add("Greater", ">");
			dict.Add("Question", "?");
			dict.Add("At", "@");
			dict.Add("Underscore", "_");
			dict.Add("A", "A");
			dict.Add("B", "B");
			dict.Add("C", "C");
			dict.Add("D", "D");
			dict.Add("E", "E");
			dict.Add("F", "F");
			dict.Add("G", "G");
			dict.Add("H", "H");
			dict.Add("I", "I");
			dict.Add("J", "J");
			dict.Add("K", "K");
			dict.Add("L", "L");
			dict.Add("M", "M");
			dict.Add("N", "N");
			dict.Add("O", "O");
			dict.Add("P", "P");
			dict.Add("Q", "Q");
			dict.Add("R", "R");
			dict.Add("S", "S");
			dict.Add("T", "T");
			dict.Add("U", "U");
			dict.Add("V", "V");
			dict.Add("W", "W");
			dict.Add("X", "X");
			dict.Add("Y", "Y");
			dict.Add("Z", "Z");
			dict.Add("Kp_minus", "-");
			dict.Add("Kp_plus", "+");
			dict.Add("Kp_enter", "\n");
			dict.Add("Comma", ",");
			dict.Add("Leftbracket", "[");
			dict.Add("Backslash", "\\");
			dict.Add("Rightbracket", "]");
			dict.Add("Caret", "^");
			dict.Add("Backquote", "`");
		}
		public static SDL2.SDL.SDL_Keycode ToSDLKeyCode(this KeyCode code)
		{
			return (SDL2.SDL.SDL_Keycode)Enum.Parse(typeof(SDL2.SDL.SDL_Keycode), code.ToString("D"));
		}
		public static KeyCode ToKeyCode(this SDL2.SDL.SDL_Keycode code)
		{
			return (KeyCode)Enum.Parse(typeof(KeyCode), code.ToString("D"));
		}
		public static string ToKeyString(this KeyCode code)
		{
			var c = code.ToString("G");
			return dict.ContainsKey(c) ? dict[c] : null;
		}
	}
}
