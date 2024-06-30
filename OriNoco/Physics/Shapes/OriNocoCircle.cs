using static SDL2.SDL;

namespace OriNoco
{
    public sealed class OriNocoCircle : OriNocoShape
    {
        #region Factory Functions
        internal void InitializeFromWorldSpace(
          Vector2 worldSpaceOrigin,
          float radius,
          float density,
          float friction,
          float restitution)
        {
            base.Initialize(density, friction, restitution);

            this.worldSpaceOrigin = worldSpaceOrigin;
            this.radius = radius;
            this.sqrRadius = radius * radius;
            this.worldSpaceAABB = new OriNocoAABB(worldSpaceOrigin, radius);
        }
        #endregion

        #region Properties
        public override ShapeType Type { get { return ShapeType.Circle; } }

        public Vector2 Origin { get { return this.worldSpaceOrigin; } }
        public float Radius { get { return this.radius; } }
        #endregion

        #region Fields
        internal Vector2 worldSpaceOrigin;
        internal float radius;
        internal float sqrRadius;

        // Precomputed body-space values (these should never change unless we
        // want to support moving shapes relative to their body root later on)
        private Vector2 bodySpaceOrigin;
        #endregion

        public OriNocoCircle()
        {
            this.Reset();
        }

        protected override void Reset()
        {
            base.Reset();

            this.worldSpaceOrigin = Vector2.zero;
            this.radius = 0.0f;
            this.sqrRadius = 0.0f;
            this.bodySpaceOrigin = Vector2.zero;
        }

        #region Functionality Overrides
        protected override void ComputeMetrics()
        {
            this.bodySpaceOrigin =
              this.Body.WorldToBodyPointCurrent(this.worldSpaceOrigin);
            this.bodySpaceAABB = new OriNocoAABB(this.bodySpaceOrigin, this.radius);

            this.Area = this.sqrRadius * Mathf.PI;
            this.Mass = this.Area * this.Density * OriNocoConfig.AreaMassRatio;
            this.Inertia =
              this.sqrRadius / 2.0f + this.bodySpaceOrigin.sqrMagnitude;
        }

        protected override void ApplyBodyPosition()
        {
            this.worldSpaceOrigin =
              this.Body.BodyToWorldPointCurrent(this.bodySpaceOrigin);
            this.worldSpaceAABB = new OriNocoAABB(this.worldSpaceOrigin, this.radius);
        }
        #endregion

        #region Test Overrides
        protected override bool ShapeQueryPoint(
          Vector2 bodySpacePoint)
        {
            return
              Collision.TestPointCircleSimple(
                this.bodySpaceOrigin,
                bodySpacePoint,
                this.radius);
        }

        protected override bool ShapeQueryCircle(
          Vector2 bodySpaceOrigin,
          float radius)
        {
            return
              Collision.TestCircleCircleSimple(
                this.bodySpaceOrigin,
                bodySpaceOrigin,
                this.radius,
                radius);
        }

        protected override bool ShapeRayCast(
          ref OriNocoRayCast bodySpaceRay,
          ref OriNocoRayResult result)
        {
            return Collision.CircleRayCast(
              this,
              this.bodySpaceOrigin,
              this.sqrRadius,
              ref bodySpaceRay,
              ref result);
        }

        protected override bool ShapeCircleCast(
          ref OriNocoRayCast bodySpaceRay,
          float radius,
          ref OriNocoRayResult result)
        {
            float totalRadius = this.radius + radius;
            return Collision.CircleRayCast(
              this,
              this.bodySpaceOrigin,
              totalRadius * totalRadius,
              ref bodySpaceRay,
              ref result);
        }
        #endregion
    }
}