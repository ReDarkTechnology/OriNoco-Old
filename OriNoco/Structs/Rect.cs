using System;
using static SDL2.SDL;
using OriNoco.Tools;

namespace OriNoco
{
	public struct Rect
	{
		/// <summary>
		/// The x origin of the rect
		/// </summary>
		public float x;
		/// <summary>
		/// The y origin of the rect
		/// </summary>
		public float y;
		/// <summary>
		/// Width of the rect
		/// </summary>
		public float w;
		/// <summary>
		/// Height of the rect
		/// </summary>
		public float h;
		/// <summary>
		/// Center point of the rect
		/// </summary>
		public SDL_FPoint c
		{
			get
			{
				return new SDL_FPoint(x + (w / 2), -y - (h / 2));
			}
		}
		/// <summary>
		/// Origin point of the rect
		/// </summary>
		public SDL_FPoint o
		{
			get
			{
				return new SDL_FPoint(x, y);
			}
		}
		/// <summary>
		/// Scale of the rect
		/// </summary>
		public SDL_FPoint s
		{
			get
			{
				return new SDL_FPoint(w, h);
			}
		}
		// Constructors
		public Rect(float x, float y, float w, float h)
		{
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
		}
		
		public Rect(SDL_FPoint position, SDL_FPoint scale)
		{
			x = position.x - scale.x / 2;
			y = -position.y - scale.y / 2;
			w = scale.x;
			h = scale.y;
		}
		
		// Conversion between SDL_Rect and this RECT
		public static implicit operator SDL_Rect(Rect v) {
			return new SDL_Rect(){
				x = v.x.ToInt(),
				y = v.y.ToInt(),
				w = v.w.ToInt(),
				h = v.h.ToInt()
			};
		}
		public static implicit operator SDL_FRect(Rect v) {
			return new SDL_FRect(){
				x = v.x,
				y = v.y,
				w = v.w,
				h = v.h
			};
		}
		
		public static implicit operator Rect(SDL_Rect v) {
			return new Rect(){
				x = v.x,
				y = v.y,
				w = v.w,
				h = v.h
			};
		}

		public override string ToString()
		{
			return string.Format("({0}, {1}, {2}, {3})", new object[] {
			                     	x.ToString(),
			                     	y.ToString(),
			                     	w.ToString(),
			                     	h.ToString()
			});
		}
		
		public static Rect Parse(string str)
		{
			var n = str.Remove(str.Length - 1, 1).Remove(0, 1);
			var s = n.Split(',');
			return new Rect(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]), float.Parse(s[3]));
		}
	}
}
