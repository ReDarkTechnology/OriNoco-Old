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
        public Vector2 offset = new Vector2(0f, 0f);

        /// <summary>
        /// The offset of the screen
        /// </summary>
        public Vector2 screenOffset = new Vector2(0f, 0f);

        /// <summary>
        /// The angle of the game environment (in degrees)
        /// </summary>
        public float rotation = 0f;

        public Viewport() { }
        public Viewport(float pixelScale, Vector2 offset, Vector2 screenOffset, float rotation)
        {
            this.pixelScale = pixelScale;
            this.offset = offset;
            this.screenOffset = screenOffset;
            this.rotation = rotation;
        }

        public Vector2 ToScreenPoint(Vector2 point)
        {
            return RotatePoint(point - offset, rotation) * pixelScale + screenOffset;
        }

        public static Vector2 RotatePoint(Vector2 point, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            return new Vector2(point.x * Mathf.Cos(radians) - point.y * Mathf.Sin(radians),
                point.x * Mathf.Sin(radians) + point.y * Mathf.Cos(radians));
        }

        public Rect ToScreenRect(Rect rect)
        {
            return new Rect(ToScreenPoint(rect.center), rect.size * pixelScale);
        }
    }
}