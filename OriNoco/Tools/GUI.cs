using System;
using OriNoco.Tools;
using SDL2;
using static SDL2.SDL;

namespace OriNoco
{
	public static class GUI
	{
		public static IntPtr font;
		
		public static IntPtr LoadFont(string path, int size = 128)
		{
			font = SDL_ttf.TTF_OpenFont(path, size);
			return font;
		}
		
		public static bool DrawButton(IntPtr renderer, Rect rect, string text, SDL_FColor color = default, SDL_FColor buttonColor = default)
		{
			var s = SDL_ttf.TTF_RenderText_Solid(font, text, color);
			var tex = SDL_CreateTextureFromSurface(renderer, s); 
			SDL_QueryTexture(tex, out var format, out var access, out var w, out var h);
			rect.w = rect.w * 100;
			rect.h = rect.h * 100;
			var r = (SDL_Rect)rect;
			var sr = new SDL_Rect() {w = w, h = h};
			var res = Utility.ResizeItemIntoCanvas(new SDL_FPoint(rect.w * 0.9f, rect.h * 0.9f), new SDL_FPoint(w, h));
			var textRect = new Rect(rect.c, res.size);
			var rc = (SDL_Rect)textRect;
			var hoverCheck = Input.IsMouseHovering(rc);
			var mPress = Input.GetKey(KeyCode.Mouse0);
			var col = buttonColor;
			if(hoverCheck) col = col + new SDL_FColor(0.03f, 0.03f, 0.03f, 0);
			if(mPress && hoverCheck) col = col + new SDL_FColor(0.12f, 0.12f, 0.12f, 0);
			SDL_SetRenderDrawColor(renderer, col);
			var butt = SDL_RenderFillRect(renderer, ref r);
			SDL_SetRenderDrawColor(renderer, color);
			SDL_RenderCopy(renderer, tex, ref sr, ref rc);
			
			// Don't forget to free your surface and texture
			SDL_FreeSurface(s);
			SDL_DestroyTexture(tex);
			return hoverCheck && Input.GetKey(KeyCode.Mouse0);
		}
	}

	public class GUIText
	{
		private uint format;
		private int access;
		public int w;
		public int h;

		private IntPtr _renderer;
		private IntPtr _textSurface;
		private IntPtr _textTexture;

		private IntPtr _font;
		public IntPtr font
		{
			get { return _font; }
			set
			{
				_font = value;
				RefreshTexture(_renderer);
			}
		}

		private string _text;
		public string text
		{
			get { return _text; }
			set
			{
				_text = value;
				RefreshTexture(_renderer);
			}
		}

		public SDL_FColor color = new SDL_FColor(1, 1, 1, 1);

		public GUIText(IntPtr renderer, IntPtr font, string text, SDL_FColor color = default)
		{
			_renderer = renderer;
			_font = font;
			_text = text;
			this.color = color;
			RefreshTexture(_renderer);
		}

		public void RefreshTexture(IntPtr renderer)
		{
			if (_textSurface != IntPtr.Zero)
				SDL_FreeSurface(_textSurface);
			if (_textTexture != IntPtr.Zero)
				SDL_DestroyTexture(_textTexture);

			_textSurface = SDL_ttf.TTF_RenderText_Solid(font, text, new SDL_Color(255, 255, 255, 255));
			_textTexture = SDL_CreateTextureFromSurface(renderer, _textSurface);
			SDL_QueryTexture(_textTexture, out format, out access, out w, out h);
		}

		public SDL_Point GetTextSize()
        {
            return new SDL_Point(w, h);
        }

		public void Draw(SDL_Rect rect)
        {
            var sourceRect = new SDL_Rect(0, 0, w, h);

			SDL_SetRenderDrawColor(_renderer, color);
            SDL_RenderCopy(_renderer, _textTexture, ref sourceRect, ref rect);
        }
	}
}