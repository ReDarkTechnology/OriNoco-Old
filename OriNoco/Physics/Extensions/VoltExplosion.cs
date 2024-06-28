using System;
using static SDL2.SDL;

namespace OriNoco
{
    public delegate void VoltExplosionCallback(
      OriNocoRayCast rayCast,
      OriNocoRayResult rayResult,
      float rayWeight);

    public partial class OriNocoWorld
    {
        // We'll increase the minimum occluder range by this amount when testing.
        // This way, if an occluder is also a target, we will catch that target
        // within the occluder range. Also allows us to handle the case where the
        // explosion origin is within both targets' and occluders' shapes.
        private const float EXPLOSION_OCCLUDER_SLOP = 0.05f;

        private OriNocoBuffer<OriNocoBody> targetBodies;
        private OriNocoBuffer<OriNocoBody> occludingBodies;

        public void PerformExplosion(
          SDL_FPoint origin,
          float radius,
          VoltExplosionCallback callback,
          VoltBodyFilter targetFilter = null,
          VoltBodyFilter occlusionFilter = null,
          int ticksBehind = 0,
          int rayCount = 32)
        {
            if (ticksBehind < 0)
                throw new ArgumentOutOfRangeException("ticksBehind");

            // Get all target bodies
            this.PopulateFiltered(
              origin,
              radius,
              targetFilter,
              ticksBehind,
              ref this.targetBodies);

            // Get all occluding bodies
            this.PopulateFiltered(
              origin,
              radius,
              occlusionFilter,
              ticksBehind,
              ref this.occludingBodies);

            OriNocoRayCast ray;
            float rayWeight = 1.0f / rayCount;
            float angleIncrement = (Mathf.PI * 2.0f) * rayWeight;

            for (int i = 0; i < rayCount; i++)
            {
                SDL_FPoint normal = OriNocoMath.Polar(angleIncrement * i);
                ray = new OriNocoRayCast(origin, normal, radius);

                float minDistance =
                  this.GetOccludingDistance(ray, ticksBehind);
                minDistance += OriNocoWorld.EXPLOSION_OCCLUDER_SLOP;

                this.TestTargets(ray, callback, ticksBehind, minDistance, rayWeight);
            }
        }

        /// <summary>
        /// Gets the distance to the closest occluder for the given ray.
        /// </summary>
        private float GetOccludingDistance(
          OriNocoRayCast ray,
          int ticksBehind)
        {
            float distance = float.MaxValue;
            OriNocoRayResult result = default(OriNocoRayResult);

            for (int i = 0; i < this.occludingBodies.Count; i++)
            {
                if (this.occludingBodies[i].RayCast(ref ray, ref result, ticksBehind))
                    distance = result.Distance;
                if (result.IsContained)
                    break;
            }

            return distance;
        }

        /// <summary>
        /// Tests all valid explosion targets for a given ray.
        /// </summary>
        private void TestTargets(
          OriNocoRayCast ray,
          VoltExplosionCallback callback,
          int ticksBehind,
          float minOccluderDistance,
          float rayWeight)
        {
            for (int i = 0; i < this.targetBodies.Count; i++)
            {
                OriNocoBody targetBody = this.targetBodies[i];
                OriNocoRayResult result = default(OriNocoRayResult);

                if (targetBody.RayCast(ref ray, ref result, ticksBehind))
                    if (result.Distance < minOccluderDistance)
                        callback.Invoke(ray, result, rayWeight);
            }
        }

        /// <summary>
        /// Finds all dynamic bodies that overlap with the explosion AABB
        /// and pass the target filter test. Does not test actual shapes.
        /// </summary>
        private void PopulateFiltered(
          SDL_FPoint origin,
          float radius,
          VoltBodyFilter targetFilter,
          int ticksBehind,
          ref OriNocoBuffer<OriNocoBody> filterBuffer)
        {
            if (filterBuffer == null)
                filterBuffer = new OriNocoBuffer<OriNocoBody>();
            filterBuffer.Clear();

            this.reusableBuffer.Clear();
            this.staticBroadphase.QueryCircle(origin, radius, this.reusableBuffer);
            this.dynamicBroadphase.QueryCircle(origin, radius, this.reusableBuffer);

            OriNocoAABB aabb = new OriNocoAABB(origin, radius);
            for (int i = 0; i < this.reusableBuffer.Count; i++)
            {
                OriNocoBody body = this.reusableBuffer[i];
                if ((targetFilter == null) || targetFilter.Invoke(body))
                    if (body.QueryAABBOnly(aabb, ticksBehind))
                        filterBuffer.Add(body);
            }
        }
    }
}