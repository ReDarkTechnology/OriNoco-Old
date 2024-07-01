using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using static SDL2.SDL;

namespace OriNoco
{
    public class TextureColored
    {
        IntPtr surface;
        public IntPtr texture;
        Dictionary<string, IntPtr> textures = new Dictionary<string, IntPtr>();

        public Rect32 sourceRect = new Rect32();
        public Vector2 pivot = new Vector2();

        public IntPtr renderer { get; private set; }

        public int w;
        public int h;

        public TextureColored(IntPtr renderer, string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            surface = SDL_image.IMG_Load(filePath);
            if (surface == IntPtr.Zero) throw new Exception("Failed to load image: " + SDL_image.IMG_GetError());

            texture = SDL_CreateTextureFromSurface(renderer, surface);
            SDL_QueryTexture(texture, out _, out _, out w, out h);
            sourceRect = new Rect32(0, 0, w, h);
            SDL_DestroyTexture(texture);

            this.renderer = renderer;
        }

        public TextureColored(IntPtr renderer, byte[] bytes)
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
                    SDL_DestroyTexture(texture);
                    textures.Add("Default", texture);

                    this.renderer = renderer;
                }
            }
        }

        /// <summary>
        /// Register a texture with a color
        /// </summary>
        /// <param name="name">The name of the colored texture</param>
        /// <param name="color">The color of the texture</param>
        /// <exception cref="MemberAccessException">The colored texture with the name already exists</exception>
        public void RegisterColor(string name, Color32 color)
        {
            if (name == "Default")
                throw new MemberAccessException("The name default has already been put for the normal texture");

            if (textures.ContainsKey(name))
                throw new MemberAccessException("The name " + name + " already exists");

            var texture = SDL_CreateTextureFromSurface(renderer, surface);
            SDL_SetTextureColorMod(texture, color.r, color.g, color.b);
            SDL_SetTextureAlphaMod(texture, color.a);
            textures.Add(name, texture);
        }

        public void Draw(string type, Rect destRect, double angle, Vector2 center, SDL_RendererFlip flipMode) =>
            SDL_RenderCopyExF(renderer, textures[type], ref sourceRect, ref destRect, angle, ref center, flipMode);

        Vector2 center = new Vector2();
        public void Draw(string type, Rect destRect, double angle)
        {
            center = destRect.size / 2;
            SDL_RenderCopyExF(renderer, textures[type], ref sourceRect, ref destRect, angle, ref center, SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }
}
