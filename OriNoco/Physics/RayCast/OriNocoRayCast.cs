using static SDL2.SDL;

namespace OriNoco
{
    /// <summary>
    /// A semi-precomputed ray optimized for fast AABB tests.
    /// </summary>
    public struct OriNocoRayCast
    {
        internal readonly SDL_FPoint origin;
        internal readonly SDL_FPoint direction;
        internal readonly SDL_FPoint invDirection;
        internal readonly float distance;
        internal readonly bool signX;
        internal readonly bool signY;

        public OriNocoRayCast(SDL_FPoint origin, SDL_FPoint destination)
        {
            SDL_FPoint delta = destination - origin;

            this.origin = origin;
            direction = delta.normalized;
            distance = delta.magnitude;
            signX = direction.x < 0.0f;
            signY = direction.y < 0.0f;
            invDirection =
              new SDL_FPoint(1.0f / direction.x, 1.0f / direction.y);
        }

        public OriNocoRayCast(SDL_FPoint origin, SDL_FPoint direction, float distance)
        {
            this.origin = origin;
            this.direction = direction;
            this.distance = distance;
            signX = direction.x < 0.0f;
            signY = direction.y < 0.0f;
            invDirection =
              new SDL_FPoint(1.0f / direction.x, 1.0f / direction.y);
        }
    }
}