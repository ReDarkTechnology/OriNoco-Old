using static SDL2.SDL;

namespace OriNoco
{
    /// <summary>
    /// A semi-precomputed ray optimized for fast AABB tests.
    /// </summary>
    public struct OriNocoRayCast
    {
        internal readonly Vector2 origin;
        internal readonly Vector2 direction;
        internal readonly Vector2 invDirection;
        internal readonly float distance;
        internal readonly bool signX;
        internal readonly bool signY;

        public OriNocoRayCast(Vector2 origin, Vector2 destination)
        {
            Vector2 delta = destination - origin;

            this.origin = origin;
            direction = delta.normalized;
            distance = delta.magnitude;
            signX = direction.x < 0.0f;
            signY = direction.y < 0.0f;
            invDirection =
              new Vector2(1.0f / direction.x, 1.0f / direction.y);
        }

        public OriNocoRayCast(Vector2 origin, Vector2 direction, float distance)
        {
            this.origin = origin;
            this.direction = direction;
            this.distance = distance;
            signX = direction.x < 0.0f;
            signY = direction.y < 0.0f;
            invDirection =
              new Vector2(1.0f / direction.x, 1.0f / direction.y);
        }
    }
}