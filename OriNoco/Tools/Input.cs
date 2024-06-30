using System;
using SDL2;
using SDLKey = SDL2.SDL.SDL_Keycode;
using static SDL2.SDL;
using System.Collections.Generic;

namespace OriNoco
{
	public static class Input
	{
		// Key Presses
		public static List<KeyStatement> statements = new List<KeyStatement>();
		public static Action<KeyCode> OnKeyDown;
		public static Action<KeyCode> OnKeyUp;
		
		public static void UpdateInput()
		{
			var keyDestroyQueue = new List<KeyStatement>();
			foreach (KeyStatement stat in statements) {
				// Wait for the input to pass one frame
				if (stat.keyTime > 0) {
					stat.keyTime--;
				} else {
					// After the frame passed, the down state is off
					stat.isDoneDown = true;
				}
				
				if (stat.isDoneDown) {
					// The mouse has been released
					if (!stat.isPressed) {
						// Wait for one frame for the up state is on
						if (!stat.isUp) {
							stat.isUp = true;
						} else {
							// After the up state is on, remove it from the list
							keyDestroyQueue.Add(stat);
						}
					}
				}
			}
			
			foreach (KeyStatement stat in keyDestroyQueue) {
				statements.Remove(stat);
			}
			
			mouseWheel = m_mW;
			m_mW = Vector2.zero;
		}
		
		public static bool GetKeyDown(KeyCode key)
		{
			var result = statements.Find(val => val.keyCode == key);
			return result != null && !result.isDoneDown;
		}
		
		public static bool GetKey(KeyCode key)
		{
			var result = statements.Find(val => val.keyCode == key);
			return result != null && !result.isUp;
		}
		
		public static bool GetKeyUp(KeyCode key)
		{
			var result = statements.Find(val => val.keyCode == key);
			return result != null && result.isUp;
		}
		
		public static bool IsStatementsHasKey(KeyCode keyCode)
		{
			bool result = false;
			foreach (KeyStatement stat in statements) {
				result |= stat.keyCode.ToString() == keyCode.ToString();
			}
			return result;
		}
		
		public static void AddKeyDown(KeyCode val)
		{
			if (!IsStatementsHasKey(val)) {
				var stat = new KeyStatement();
				if (OnKeyDown != null)
					OnKeyDown.Invoke(val);
				stat.keyCode = val;
				stat.keyTime = 1;
				stat.isPressed = true;
				statements.Add(stat);
			}
		}
		
		public static void AddKeyUp(KeyCode val)
		{
			var t = statements.Find(k => k.keyCode == val);
			
			if (t != null)
			{
				if (OnKeyUp != null)
					OnKeyUp.Invoke(val);
				t.isPressed = false;
			}
		}
		
		public static KeyCode MouseToKeyCode(byte button)
		{
			return button == 1u ? KeyCode.Mouse0 : button == 2u ? KeyCode.Mouse1 : KeyCode.Mouse2;
		}
		
		
		public static Vector2 mousePosition {
			get {
				int x, y;
				SDL.SDL_GetMouseState(out x, out y);
				return new Vector2(x, y);
			}
		}
		public static Vector2 m_mW;
		public static Vector2 mouseWheel;
		public static bool IsMouseHovering(Rect a)
		{
			var b = new Rect(mousePosition.x - 1, mousePosition.y - 1, 2, 2);
			return a.x + a.w > b.x && a.x < b.x + b.w && a.y + a.h > b.y && a.y < b.y + b.h;
		}
	}
	[Serializable]
	public class KeyStatement
	{
		public KeyCode keyCode;
		public int keyTime = 1;
		public bool isPressed;
		public bool isDoneDown;
		public bool isUp;
	}
}