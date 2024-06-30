using static SDL2.SDL;

namespace OriNoco
{
    /// <summary>
    /// A stored historical image of a past body state, used for historical
    /// queries and raycasts. Rather than actually rolling the body back to
    /// its old position (expensive), we transform the ray into the body's
    /// local space based on the body's old position/axis. Then all casts
    /// on shapes use the local-space ray (this applies both for current-
    /// time and past-time raycasts and point queries).
    /// </summary>
    internal struct HistoryRecord
    {
        internal OriNocoAABB aabb;
        internal Vector2 position;
        internal Vector2 facing;

        internal void Store(ref HistoryRecord other)
        {
            this.aabb = other.aabb;
            this.position = other.position;
            this.facing = other.facing;
        }

        #region World-Space to Body-Space Transformations
        internal Vector2 WorldToBodyPoint(Vector2 vector)
        {
            return OriNocoMath.WorldToBodyPoint(this.position, this.facing, vector);
        }

        internal Vector2 WorldToBodyDirection(Vector2 vector)
        {
            return OriNocoMath.WorldToBodyDirection(this.facing, vector);
        }

        internal OriNocoRayCast WorldToBodyRay(ref OriNocoRayCast rayCast)
        {
            return new OriNocoRayCast(
              this.WorldToBodyPoint(rayCast.origin),
              this.WorldToBodyDirection(rayCast.direction),
              rayCast.distance);
        }
        #endregion

        #region Body-Space to World-Space Transformations
        internal Vector2 BodyToWorldPoint(Vector2 vector)
        {
            return OriNocoMath.BodyToWorldPoint(this.position, this.facing, vector);
        }

        internal Vector2 BodyToWorldDirection(Vector2 vector)
        {
            return OriNocoMath.BodyToWorldDirection(this.facing, vector);
        }

        internal Axis BodyToWorldAxis(Axis axis)
        {
            Vector2 normal = axis.Normal.Rotate(this.facing);
            float width = Vector2.Dot(normal, this.position) + axis.Width;
            return new Axis(normal, width);
        }
        #endregion
    }
}