using System;
using System.IO;
using SDL2;
using static SDL2.SDL;

namespace OriNoco
{
    public class TextureRenderer
    {
        IntPtr surface;
        IntPtr texture;

        private SDL_Color _color = new SDL_Color(255, 255, 255, 255);
        public SDL_Color color
        {
            get => _color;
            set
            {
                if (_color == value) return;
                _color = value;
                ChangeColor(_color);
            }
        }

        public SDL_Rect sourceRect = new SDL_Rect();
        public SDL_FPoint pivot = new SDL_FPoint();

        private IntPtr _renderer;
        public IntPtr renderer
        {
            get => _renderer;
            set
            {
                SDL_DestroyTexture(texture);
                texture = SDL_CreateTextureFromSurface(value, surface);
                _renderer = value;
            }
        }

        public TextureRenderer(IntPtr renderer, string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            surface = SDL_image.IMG_Load(filePath);
            if (surface == IntPtr.Zero) throw new Exception("Failed to load image: " + SDL_image.IMG_GetError());

            texture = SDL_CreateTextureFromSurface(renderer, surface);
            SDL_QueryTexture(texture, out _, out _, out sourceRect.w, out sourceRect.h);

            _renderer = renderer;
        }

        public TextureRenderer(IntPtr renderer, byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0) throw new ArgumentException(nameof(bytes) + " does not have any data");

            unsafe
            {
                fixed (byte* pData = bytes)
                {
                    IntPtr dataPtr = new(pData);

                    surface = SDL_image.IMG_Load_RW(SDL_RWFromMem(dataPtr, bytes.Length), 1);
                    if (surface == IntPtr.Zero) throw new Exception("Failed to load image: " + SDL_image.IMG_GetError());

                    texture = SDL_CreateTextureFromSurface(renderer, surface);
                    SDL_QueryTexture(texture, out _, out _, out sourceRect.w, out sourceRect.h);

                    _renderer = renderer;
                }
            }
        }

        private void ChangeColor(SDL_Color color)
        {
            SDL_DestroyTexture(texture);

            SDL_SetSurfaceColorMod(surface, color.r, color.g, color.b);
            SDL_SetSurfaceAlphaMod(surface, color.a);
            texture = SDL_CreateTextureFromSurface(renderer, surface);
        }

        public void Draw(SDL_FRect destRect, double angle, SDL_FPoint center, SDL_RendererFlip flipMode) =>
            SDL_RenderCopyExF(renderer, texture, ref sourceRect, ref destRect, angle, ref center, flipMode);

        SDL_FPoint center = new SDL_FPoint();
        public void Draw(SDL_FRect destRect, double angle)
        {
            center = destRect.size / 2;
            SDL_SetRenderDrawColor(renderer, _color);
            SDL_RenderCopyExF(renderer, texture, ref sourceRect, ref destRect, angle, ref center, SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }
}
