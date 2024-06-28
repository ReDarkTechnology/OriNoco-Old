using static SDL2.SDL;

namespace OriNoco
{
  public struct OriNocoAABB
  {
    #region Static Methods
    public static OriNocoAABB CreateExpanded(OriNocoAABB aabb, float expansionAmount)
    {
      return new OriNocoAABB(
        aabb.top + expansionAmount,
        aabb.bottom - expansionAmount,
        aabb.left - expansionAmount,
        aabb.right + expansionAmount);
    }

    public static OriNocoAABB CreateMerged(OriNocoAABB aabb1, OriNocoAABB aabb2)
    {
      return new OriNocoAABB(
        Mathf.Max(aabb1.top, aabb2.top),
        Mathf.Min(aabb1.bottom, aabb2.bottom),
        Mathf.Min(aabb1.left, aabb2.left),
        Mathf.Max(aabb1.right, aabb2.right));
    }

    public static OriNocoAABB CreateSwept(OriNocoAABB source, SDL_FPoint vector)
    {
      float top = source.top;
      float bottom = source.bottom;
      float left = source.left;
      float right = source.right;

      if (vector.x < 0.0f)
        left += vector.x;
      else
        right += vector.x;

      if (vector.y < 0.0f)
        bottom += vector.y;
      else
        top += vector.y;

      return new OriNocoAABB(top, bottom, left, right);
    }

    /// <summary>
    /// A cheap ray test that requires some precomputed information.
    /// Adapted from: http://www.cs.utah.edu/~awilliam/box/box.pdf
    /// </summary>
    private static bool RayCast(
      ref OriNocoRayCast ray,
      float top,
      float bottom,
      float left,
      float right)
    {
      float txmin =
        ((ray.signX ? right : left) - ray.origin.x) *
        ray.invDirection.x;
      float txmax =
        ((ray.signX ? left : right) - ray.origin.x) *
        ray.invDirection.x;

      float tymin =
        ((ray.signY ? top : bottom) - ray.origin.y) *
        ray.invDirection.y;
      float tymax =
        ((ray.signY ? bottom : top) - ray.origin.y) *
        ray.invDirection.y;

      if ((txmin > tymax) || (tymin > txmax))
        return false;
      if (tymin > txmin)
        txmin = tymin;
      if (tymax < txmax)
        txmax = tymax;
      return (txmax > 0.0f) && (txmin < ray.distance);
    }
    #endregion

    public SDL_FPoint TopLeft 
    { 
      get { return new SDL_FPoint(this.left, this.top); } 
    }

    public SDL_FPoint TopRight 
    { 
      get { return new SDL_FPoint(this.right, this.top); } 
    }

    public SDL_FPoint BottomLeft 
    { 
      get { return new SDL_FPoint(this.left, this.bottom); } 
    }

    public SDL_FPoint BottomRight 
    { 
      get { return new SDL_FPoint(this.right, this.bottom); } 
    }

    public float Top { get { return this.top; } }
    public float Bottom { get { return this.bottom; } }
    public float Left { get { return this.left; } }
    public float Right { get { return this.right; } }

    public float Width { get { return this.Right - this.Left; } }
    public float Height { get { return this.Top - this.Bottom; } }

    public float Area { get { return this.Width * this.Height; } }
    public float Perimeter 
    { 
      get { return 2.0f * (this.Width + this.Height); } 
    }

    public SDL_FPoint Center { get { return this.ComputeCenter(); } }
    public SDL_FPoint Extent 
    { 
      get { return new SDL_FPoint(this.Width * 0.5f, this.Height * 0.5f); } 
    }

    private readonly float top;
    private readonly float bottom;
    private readonly float left;
    private readonly float right;

    #region Tests
    /// <summary>
    /// Performs a point test on the AABB.
    /// </summary>
    public bool QueryPoint(SDL_FPoint point)
    {
      return 
        left <= point.x && 
        right >= point.x &&
        bottom <= point.y &&
        top >= point.y;
    }

    /// <summary>
    /// Note: This doesn't take rounded edges into account.
    /// </summary>
    public bool QueryCircleApprox(SDL_FPoint origin, float radius)
    {
      return
        (left - radius) <= origin.x &&
        (right + radius) >= origin.x &&
        (bottom - radius) <= origin.y &&
        (top + radius) >= origin.y;
    }

    public bool RayCast(ref OriNocoRayCast ray)
    {
      return RayCast(
        ref ray, 
        top, 
        bottom, 
        left, 
        right);
    }

    /// <summary>
    /// Note: This doesn't take rounded edges into account.
    /// </summary>
    public bool CircleCastApprox(ref OriNocoRayCast ray, float radius)
    {
      return RayCast(
        ref ray,
        top + radius,
        bottom - radius,
        left - radius,
        right + radius);
    }

    public bool Intersect(OriNocoAABB other)
    {
      bool outside =
        right <= other.left ||
        left >= other.right ||
        bottom >= other.top ||
        top <= other.bottom;
      return (outside == false);
    }

    public bool Contains(OriNocoAABB other)
    {
      return
        top >= other.Top &&
        bottom <= other.Bottom &&
        right >= other.right &&
        left <= other.left;
    }
    #endregion

    public OriNocoAABB(float top, float bottom, float left, float right)
    {
      this.top = top;
      this.bottom = bottom;
      this.left = left;
      this.right = right;
    }

    public OriNocoAABB(SDL_FPoint center, SDL_FPoint extents)
    {
      SDL_FPoint topRight = center + extents;
      SDL_FPoint bottomLeft = center - extents;

      top = topRight.y;
      right = topRight.x;
      bottom = bottomLeft.y;
      left = bottomLeft.x;
    }

    public OriNocoAABB(SDL_FPoint center, float radius)
      : this (center, new SDL_FPoint(radius, radius))
    {
    }

    public OriNocoAABB ComputeTopLeft(SDL_FPoint center)
    {
      return new OriNocoAABB(top, center.y, left, center.x);
    }

    public OriNocoAABB ComputeTopRight(SDL_FPoint center)
    {
      return new OriNocoAABB(top, center.y, center.x, right);
    }

    public OriNocoAABB ComputeBottomLeft(SDL_FPoint center)
    {
      return new OriNocoAABB(center.y, bottom, left, center.x);
    }

    public OriNocoAABB ComputeBottomRight(SDL_FPoint center)
    {
      return new OriNocoAABB(center.y, bottom, center.x, right);
    }

    private SDL_FPoint ComputeCenter()
    {
      return new SDL_FPoint(
        (Width * 0.5f) + left, 
        (Height * 0.5f) + bottom);
    }
  }
}
