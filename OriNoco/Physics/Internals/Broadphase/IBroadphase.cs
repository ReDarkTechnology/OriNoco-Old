using static SDL2.SDL;

namespace OriNoco
{
  internal interface IBroadPhase
  {
    void AddBody(OriNocoBody body);
    void RemoveBody(OriNocoBody body);
    void UpdateBody(OriNocoBody body);

    // Note that these should return bodies that meet the criteria within the
    // spaces defined by the structure itself. These tests should not test the
    // actual body's bounding box, as that will happen in the beginning of the
    // narrowphase test.
    void QueryOverlap(OriNocoAABB aabb, OriNocoBuffer<OriNocoBody> outBuffer);
    void QueryPoint(Vector2 point, OriNocoBuffer<OriNocoBody> outBuffer);
    void QueryCircle(Vector2 point, float radius, OriNocoBuffer<OriNocoBody> outBuffer);
    void RayCast(ref OriNocoRayCast ray, OriNocoBuffer<OriNocoBody> outBuffer);
    void CircleCast(ref OriNocoRayCast ray, float radius, OriNocoBuffer<OriNocoBody> outBuffer);
  }
}