using static SDL2.SDL;

namespace OriNoco
{
  public abstract class OriNocoShape
    : IOriNocoPoolable<OriNocoShape>
  {
    #region Interface
    IOriNocoPool<OriNocoShape> IOriNocoPoolable<OriNocoShape>.Pool { get; set; }
    void IOriNocoPoolable<OriNocoShape>.Reset() { this.Reset(); }
    #endregion

    #region Static Methods
    internal static void OrderShapes(ref OriNocoShape sa, ref OriNocoShape sb)
    {
      if (sa.Type > sb.Type)
      {
        OriNocoShape temp = sa;
        sa = sb;
        sb = temp;
      }
    }
    #endregion

    public enum ShapeType
    {
      Circle,
      Polygon,
    }

#if DEBUG
    internal bool IsInitialized { get; set; }
#endif

    public abstract ShapeType Type { get; }

    /// <summary>
    /// For attaching arbitrary data to this shape.
    /// </summary>
    public object UserData { get; set; }
    public OriNocoBody Body { get; private set; }

    internal float Density { get; private set; }
    internal float Friction { get; private set; }
    internal float Restitution { get; private set; }

    /// <summary>
    /// The world-space bounding AABB for this shape.
    /// </summary>
    public OriNocoAABB AABB { get { return this.worldSpaceAABB; } }

    /// <summary>
    /// Total area of the shape.
    /// </summary>
    public float Area { get; protected set; }

    /// <summary>
    /// Total mass of the shape (area * density).
    /// </summary>
    public float Mass { get; protected set; }

    /// <summary>
    /// Total inertia of the shape relative to the body's origin.
    /// </summary>
    public float Inertia { get; protected set; }

    // Body-space bounding AABB for pre-checks during queries/casts
    internal OriNocoAABB worldSpaceAABB;
    internal OriNocoAABB bodySpaceAABB;

    #region Body-Related
    internal void AssignBody(OriNocoBody body)
    {
      this.Body = body;
      this.ComputeMetrics();
    }

    internal void OnBodyPositionUpdated()
    {
      this.ApplyBodyPosition();
    }
    #endregion

    #region Tests
    /// <summary>
    /// Checks if a point is contained in this shape. 
    /// Begins with an AABB check.
    /// </summary>
    internal bool QueryPoint(SDL_FPoint bodySpacePoint)
    {
      // Queries and casts on shapes are always done in body space
      if (this.bodySpaceAABB.QueryPoint(bodySpacePoint))
        return this.ShapeQueryPoint(bodySpacePoint);
      return false;
    }

    /// <summary>
    /// Checks if a circle overlaps with this shape. 
    /// Begins with an AABB check.
    /// </summary>
    internal bool QueryCircle(SDL_FPoint bodySpaceOrigin, float radius)
    {
      // Queries and casts on shapes are always done in body space
      if (this.bodySpaceAABB.QueryCircleApprox(bodySpaceOrigin, radius))
        return this.ShapeQueryCircle(bodySpaceOrigin, radius);
      return false;
    }

    /// <summary>
    /// Performs a raycast check on this shape. 
    /// Begins with an AABB check.
    /// </summary>
    internal bool RayCast(
      ref OriNocoRayCast bodySpaceRay, 
      ref OriNocoRayResult result)
    {
      // Queries and casts on shapes are always done in body space
      if (this.bodySpaceAABB.RayCast(ref bodySpaceRay))
        return this.ShapeRayCast(ref bodySpaceRay, ref result);
      return false;
    }

    /// <summary>
    /// Performs a circlecast check on this shape. 
    /// Begins with an AABB check.
    /// </summary>
    internal bool CircleCast(
      ref OriNocoRayCast bodySpaceRay, 
      float radius, 
      ref OriNocoRayResult result)
    {
      // Queries and casts on shapes are always done in body space
      if (this.bodySpaceAABB.CircleCastApprox(ref bodySpaceRay, radius))
        return this.ShapeCircleCast(ref bodySpaceRay, radius, ref result);
      return false;
    }
    #endregion

    protected void Initialize(
      float density, 
      float friction, 
      float restitution)
    {
      this.Density = density;
      this.Friction = friction;
      this.Restitution = restitution;

#if DEBUG
      this.IsInitialized = true;
#endif
    }

    protected virtual void Reset()
    {
#if DEBUG
      this.IsInitialized = false;
#endif

      this.UserData = null;
      this.Body = null;

      this.Density = 0.0f;
      this.Friction = 0.0f;
      this.Restitution = 0.0f;

      this.Area = 0.0f;
      this.Mass = 0.0f;
      this.Inertia = 0.0f;

      this.bodySpaceAABB = default(OriNocoAABB);
      this.worldSpaceAABB = default(OriNocoAABB);
    }

    #region Functionality Overrides
    protected abstract void ComputeMetrics();
    protected abstract void ApplyBodyPosition();
    #endregion

    #region Test Overrides
    protected abstract bool ShapeQueryPoint(
      SDL_FPoint bodySpacePoint);

    protected abstract bool ShapeQueryCircle(
      SDL_FPoint bodySpaceOrigin,
      float radius);

    protected abstract bool ShapeRayCast(
      ref OriNocoRayCast bodySpaceRay,
      ref OriNocoRayResult result);

    protected abstract bool ShapeCircleCast(
      ref OriNocoRayCast bodySpaceRay,
      float radius,
      ref OriNocoRayResult result);
    #endregion

    #region Debug
#if UNITY && DEBUG
    public abstract void GizmoDraw(
      Color edgeColor,
      Color normalColor,
      Color originColor,
      Color aabbColor,
      float normalLength);
#endif
    #endregion
  }
}