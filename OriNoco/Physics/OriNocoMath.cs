using static SDL2.SDL;

namespace OriNoco
{
    public static class OriNocoMath
    {
        #region Transformations
        public static Vector2 WorldToBodyPoint(
          Vector2 bodyPosition,
          Vector2 bodyFacing,
          Vector2 vector)
        {
            return (vector - bodyPosition).InvRotate(bodyFacing);
        }

        public static Vector2 WorldToBodyDirection(
          Vector2 bodyFacing,
          Vector2 vector)
        {
            return vector.InvRotate(bodyFacing);
        }
        #endregion

        #region Body-Space to World-Space Transformations
        public static Vector2 BodyToWorldPoint(
          Vector2 bodyPosition,
          Vector2 bodyFacing,
          Vector2 vector)
        {
            return vector.Rotate(bodyFacing) + bodyPosition;
        }

        public static Vector2 BodyToWorldDirection(
          Vector2 bodyFacing,
          Vector2 vector)
        {
            return vector.Rotate(bodyFacing);
        }
        #endregion

        public static Vector2 Right(this Vector2 v)
        {
            return new Vector2(v.y, -v.x);
        }

        public static Vector2 Left(this Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }

        public static Vector2 Rotate(this Vector2 v, Vector2 b)
        {
            return new Vector2(v.x * b.x - v.y * b.y, v.y * b.x + v.x * b.y);
        }

        public static Vector2 InvRotate(this Vector2 v, Vector2 b)
        {
            return new Vector2(v.x * b.x + v.y * b.y, v.y * b.x - v.x * b.y);
        }

        public static float Angle(this Vector2 v)
        {
            return Mathf.Atan2(v.y, v.x);
        }

        public static Vector2 Polar(float radians)
        {
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static float Square(float a)
        {
            return a * a;
        }
    }
}