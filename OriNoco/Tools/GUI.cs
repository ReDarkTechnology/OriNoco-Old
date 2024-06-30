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

		public Color color = new Color(1, 1, 1, 1);

		public GUIText(IntPtr renderer, IntPtr font, string text, Color color = default)
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

			_textSurface = SDL_ttf.TTF_RenderText_Solid(font, text, new Color32(255, 255, 255, 255));
			_textTexture = SDL_CreateTextureFromSurface(renderer, _textSurface);
			SDL_QueryTexture(_textTexture, out format, out access, out w, out h);
		}

		public Point GetTextSize()
        {
            return new Point(w, h);
        }

		public void Draw(Rect32 rect)
        {
            var sourceRect = new Rect32(0, 0, w, h);

			SDL_SetRenderDrawColor(_renderer, color);
            SDL_RenderCopy(_renderer, _textTexture, ref sourceRect, ref rect);
        }
	}
}