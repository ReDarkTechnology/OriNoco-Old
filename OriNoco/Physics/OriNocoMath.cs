using static SDL2.SDL;

namespace OriNoco
{
    public static class OriNocoMath
    {
        #region Transformations
        public static SDL_FPoint WorldToBodyPoint(
          SDL_FPoint bodyPosition,
          SDL_FPoint bodyFacing,
          SDL_FPoint vector)
        {
            return (vector - bodyPosition).InvRotate(bodyFacing);
        }

        public static SDL_FPoint WorldToBodyDirection(
          SDL_FPoint bodyFacing,
          SDL_FPoint vector)
        {
            return vector.InvRotate(bodyFacing);
        }
        #endregion

        #region Body-Space to World-Space Transformations
        public static SDL_FPoint BodyToWorldPoint(
          SDL_FPoint bodyPosition,
          SDL_FPoint bodyFacing,
          SDL_FPoint vector)
        {
            return vector.Rotate(bodyFacing) + bodyPosition;
        }

        public static SDL_FPoint BodyToWorldDirection(
          SDL_FPoint bodyFacing,
          SDL_FPoint vector)
        {
            return vector.Rotate(bodyFacing);
        }
        #endregion

        public static SDL_FPoint Right(this SDL_FPoint v)
        {
            return new SDL_FPoint(v.y, -v.x);
        }

        public static SDL_FPoint Left(this SDL_FPoint v)
        {
            return new SDL_FPoint(-v.y, v.x);
        }

        public static SDL_FPoint Rotate(this SDL_FPoint v, SDL_FPoint b)
        {
            return new SDL_FPoint(v.x * b.x - v.y * b.y, v.y * b.x + v.x * b.y);
        }

        public static SDL_FPoint InvRotate(this SDL_FPoint v, SDL_FPoint b)
        {
            return new SDL_FPoint(v.x * b.x + v.y * b.y, v.y * b.x - v.x * b.y);
        }

        public static float Angle(this SDL_FPoint v)
        {
            return Mathf.Atan2(v.y, v.x);
        }

        public static SDL_FPoint Polar(float radians)
        {
            return new SDL_FPoint(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        public static float Cross(SDL_FPoint a, SDL_FPoint b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static float Square(float a)
        {
            return a * a;
        }
    }
}