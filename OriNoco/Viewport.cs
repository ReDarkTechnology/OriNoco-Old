using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using static SDL2.SDL;

namespace OriNoco
{
    /// <summary>
    /// A class to manipulate vectors and floats to adjust on the screen
    /// </summary>
    public class Viewport
    {
        /// <summary>
        /// Ratio of screen pixel and game pixel
        /// </summary>
        public float pixelScale = 1f;

        /// <summary>
        /// The offset of the game environment
        /// </summary>
        public SDL_FPoint offset = new SDL_FPoint(0f, 0f);

        /// <summary>
        /// The offset of the screen
        /// </summary>
        public SDL_FPoint screenOffset = new SDL_FPoint(0f, 0f);

        /// <summary>
        /// The angle of the game environment (in degrees)
        /// </summary>
        public float rotation = 0f;

        public Viewport() { }
        public Viewport(float pixelScale, SDL_FPoint offset, SDL_FPoint screenOffset, float rotation)
        {
            this.pixelScale = pixelScale;
            this.offset = offset;
            this.screenOffset = screenOffset;
            this.rotation = rotation;
        }

        public SDL_FPoint ToScreenPoint(SDL_FPoint point)
        {
            return RotatePoint(point - offset, rotation) * pixelScale + screenOffset;
        }

        public static SDL_FPoint RotatePoint(SDL_FPoint point, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            return new SDL_FPoint(point.x * Mathf.Cos(radians) - point.y * Mathf.Sin(radians),
                point.x * Mathf.Sin(radians) + point.y * Mathf.Cos(radians));
        }

        public SDL_FRect ToScreenRect(SDL_FRect rect)
        {
            return new SDL_FRect(ToScreenPoint(rect.center), rect.size * pixelScale);
        }
    }
}