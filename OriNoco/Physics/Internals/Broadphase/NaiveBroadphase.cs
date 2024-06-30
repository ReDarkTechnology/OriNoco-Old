using static SDL2.SDL;

namespace OriNoco
{
    internal class NaiveBroadphase : IBroadPhase
    {
        private OriNocoBody[] bodies;
        private int count;

        public NaiveBroadphase()
        {
            this.bodies = new OriNocoBody[256];
            this.count = 0;
        }

        public void AddBody(OriNocoBody body)
        {
            if (this.count >= this.bodies.Length)
                OriNocoUtil.ExpandArray(ref this.bodies);

            this.bodies[this.count] = body;
            body.ProxyId = this.count;
            this.count++;
        }

        public void RemoveBody(OriNocoBody body)
        {
            int index = body.ProxyId;
            OriNocoDebug.Assert(index >= 0);
            OriNocoDebug.Assert(index < this.count);

            int lastIndex = this.count - 1;
            if (index < lastIndex)
            {
                OriNocoBody lastBody = this.bodies[lastIndex];

                this.bodies[lastIndex].ProxyId = -1;
                this.bodies[lastIndex] = null;

                this.bodies[index] = lastBody;
                lastBody.ProxyId = index;
            }

            this.count--;
        }

        public void UpdateBody(OriNocoBody body)
        {
            // Do nothing
        }

        public void QueryOverlap(
          OriNocoAABB aabb,
          OriNocoBuffer<OriNocoBody> outBuffer)
        {
            outBuffer.Add(this.bodies, this.count);
        }

        public void QueryPoint(
          Vector2 point,
          OriNocoBuffer<OriNocoBody> outBuffer)
        {
            outBuffer.Add(this.bodies, this.count);
        }

        public void QueryCircle(
          Vector2 point,
          float radius,
          OriNocoBuffer<OriNocoBody> outBuffer)
        {
            outBuffer.Add(this.bodies, this.count);
        }

        public void RayCast(
          ref OriNocoRayCast ray,
          OriNocoBuffer<OriNocoBody> outBuffer)
        {
            outBuffer.Add(this.bodies, this.count);
        }

        public void CircleCast(
          ref OriNocoRayCast ray,
          float radius,
          OriNocoBuffer<OriNocoBody> outBuffer)
        {
            outBuffer.Add(this.bodies, this.count);
        }
    }
}