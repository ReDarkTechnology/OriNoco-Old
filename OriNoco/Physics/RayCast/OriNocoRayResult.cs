using static SDL2.SDL;

namespace OriNoco
{
    public struct OriNocoRayResult
    {
        public bool IsValid { get { return this.shape != null; } }
        public bool IsContained
        {
            get { return this.IsValid && this.distance == 0.0f; }
        }

        public OriNocoShape Shape { get { return this.shape; } }

        public OriNocoBody Body
        {
            get { return (this.shape == null) ? null : this.shape.Body; }
        }

        public float Distance { get { return this.distance; } }
        public SDL_FPoint Normal { get { return this.normal; } }

        private OriNocoShape shape;
        private float distance;
        internal SDL_FPoint normal;

        public SDL_FPoint ComputePoint(ref OriNocoRayCast cast)
        {
            return cast.origin + (cast.direction * this.distance);
        }

        internal void Set(
          OriNocoShape shape,
          float distance,
          SDL_FPoint normal)
        {
            if (this.IsValid == false || distance < this.distance)
            {
                this.shape = shape;
                this.distance = distance;
                this.normal = normal;
            }
        }

        internal void Reset()
        {
            this.shape = null;
        }

        internal void SetContained(OriNocoShape shape)
        {
            this.shape = shape;
            this.distance = 0.0f;
            this.normal = SDL_FPoint.zero;
        }
    }
}