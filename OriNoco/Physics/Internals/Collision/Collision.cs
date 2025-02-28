﻿using static SDL2.SDL;

namespace OriNoco
{
  internal static class Collision
  {
    #region Dispatch
    private delegate Manifold Test(
      OriNocoWorld world,
      OriNocoShape sa, 
      OriNocoShape sb);

    private readonly static Test[,] tests = new Test[,]
      {
        { __Circle_Circle, __Circle_Polygon },
        { __Polygon_Circle, __Polygon_Polygon}
      };

    internal static Manifold Dispatch(
      OriNocoWorld world,
      OriNocoShape sa, 
      OriNocoShape sb)
    {
      Test test = Collision.tests[(int)sa.Type, (int)sb.Type];
      return test(world, sa, sb);
    }

    private static Manifold __Circle_Circle(
      OriNocoWorld world,
      OriNocoShape sa, 
      OriNocoShape sb)
    {
      return Circle_Circle(world, (OriNocoCircle)sa, (OriNocoCircle)sb);
    }

    private static Manifold __Circle_Polygon(
      OriNocoWorld world,
      OriNocoShape sa, 
      OriNocoShape sb)
    {
      return Circle_Polygon(world, (OriNocoCircle)sa, (OriNocoPolygon)sb);
    }

    private static Manifold __Polygon_Circle(
      OriNocoWorld world,
      OriNocoShape sa, 
      OriNocoShape sb)
    {
      return Circle_Polygon(world, (OriNocoCircle)sb, (OriNocoPolygon)sa);
    }

    private static Manifold __Polygon_Polygon(
      OriNocoWorld world,
      OriNocoShape sa, 
      OriNocoShape sb)
    {
      return Polygon_Polygon(world, (OriNocoPolygon)sa, (OriNocoPolygon)sb);
    }
    #endregion

    #region Collision Tests
    private static Manifold Circle_Circle(
      OriNocoWorld world,
      OriNocoCircle circA,
      OriNocoCircle circB)
    {
      return 
        TestCircles(
          world,
          circA, 
          circB,
          circB.worldSpaceOrigin, 
          circB.radius);
    }

    private static Manifold Circle_Polygon(
      OriNocoWorld world,
      OriNocoCircle circ,
      OriNocoPolygon poly)
    {
      // Get the axis on the polygon closest to the circle's origin
      float penetration;
      int index =
        Collision.FindAxisMaxPenetration(
          circ.worldSpaceOrigin,
          circ.radius,
          poly,
          out penetration);

      if (index < 0)
        return null;

      Vector2 a, b;
      poly.GetEdge(index, out a, out b);
      Axis axis = poly.GetWorldAxis(index);

      // If the circle is past one of the two vertices, check it like
      // a circle-circle intersection where the vertex has radius 0
      float d = OriNocoMath.Cross(axis.Normal, circ.worldSpaceOrigin);
      if (d > OriNocoMath.Cross(axis.Normal, a))
        return Collision.TestCircles(world, circ, poly, a, 0.0f);
      if (d < OriNocoMath.Cross(axis.Normal, b))
        return Collision.TestCircles(world, circ, poly, b, 0.0f);

      // Build the collision Manifold
      Manifold manifold = world.AllocateManifold().Assign(world, circ, poly);
      Vector2 pos =
        circ.worldSpaceOrigin - (circ.radius + penetration / 2) * axis.Normal;
      manifold.AddContact(pos, -axis.Normal, penetration);
      return manifold;
    }

    private static Manifold Polygon_Polygon(
      OriNocoWorld world,
      OriNocoPolygon polyA,
      OriNocoPolygon polyB)
    {
      Axis a1, a2;
      if (Collision.FindMinSepAxis(polyA, polyB, out a1) == false)
        return null;
      if (Collision.FindMinSepAxis(polyB, polyA, out a2) == false)
        return null;

      // We will use poly1's axis, so we may need to swap
      if (a2.Width > a1.Width)
      {
        OriNocoUtil.Swap(ref polyA, ref polyB);
        OriNocoUtil.Swap(ref a1, ref a2);
      }

      // Build the collision Manifold
      Manifold manifold = 
        world.AllocateManifold().Assign(world, polyA, polyB);
      Collision.FindVerts(polyA, polyB, a1.Normal, a1.Width, manifold);
      return manifold;
    }
    #endregion

    #region Common Tests and Queries
    /// <summary>
    /// Simple check for point-circle containment.
    /// </summary>
    internal static bool TestPointCircleSimple(
      Vector2 point,
      Vector2 origin,
      float radius)
    {
      Vector2 delta = origin - point;
      return delta.sqrMagnitude <= (radius * radius);
    }

    /// <summary>
    /// Simple check for two overlapping circles.
    /// </summary>
    internal static bool TestCircleCircleSimple(
      Vector2 originA,
      Vector2 originB,
      float radiusA,
      float radiusB)
    {
      float radiusTotal = radiusA + radiusB;
      return (originA - originB).sqrMagnitude <= (radiusTotal * radiusTotal);
    }

    /// <summary>
    /// Checks a ray against a circle with a given origin and square radius.
    /// </summary>
    internal static bool CircleRayCast(
      OriNocoShape shape,
      Vector2 shapeOrigin,
      float sqrRadius,
      ref OriNocoRayCast ray,
      ref OriNocoRayResult result)
    {
      Vector2 toOrigin = shapeOrigin - ray.origin;

      if (toOrigin.sqrMagnitude < sqrRadius)
      {
        result.SetContained(shape);
        return true;
      }

      float slope = Vector2.Dot(toOrigin, ray.direction);
      if (slope < 0)
        return false;

      float sqrSlope = slope * slope;
      float d = sqrRadius + sqrSlope - Vector2.Dot(toOrigin, toOrigin);
      if (d < 0)
        return false;

      float dist = slope - Mathf.Sqrt(d);
      if (dist < 0 || dist > ray.distance)
        return false;

      // N.B.: For historical raycasts this normal will be wrong!
      // Must be either transformed back to world or invalidated later.
      Vector2 normal = (dist * ray.direction - toOrigin).normalized;
      result.Set(shape, dist, normal);
      return true;
    }


    /// <summary>
    /// Returns the index of the nearest axis on the poly to a point.
    /// Outputs the minimum distance between the axis and the point.
    /// </summary>
    internal static int FindAxisShortestDistance(
      Vector2 point,
      Axis[] axes,
      out float minDistance)
    {
      int ix = 0;
      minDistance = float.PositiveInfinity;
      bool inside = true;

      for (int i = 0; i < axes.Length; i++)
      {
        float dot = Vector2.Dot(axes[i].Normal, point);
        float dist = axes[i].Width - dot;

        if (dist < 0.0f)
          inside = false;

        if (dist < minDistance)
        {
          minDistance = dist;
          ix = i;
        }
      }

      if (inside == true)
      {
        minDistance = 0.0f;
        ix = -1;
      }

      return ix;
    }

    /// <summary>
    /// Returns the index of the axis with the max circle penetration depth.
    /// Breaks out if a separating axis is found between the two shapes.
    /// Outputs the penetration depth of the circle in the axis (if any).
    /// </summary>
    internal static int FindAxisMaxPenetration(
      Vector2 origin,
      float radius,
      OriNocoPolygon poly,
      out float penetration)
    {
      int index = 0;
      int found = 0;
      penetration = float.NegativeInfinity;

      for (int i = 0; i < poly.countWorld; i++)
      {
        Axis axis = poly.worldAxes[i];
        float dot = Vector2.Dot(axis.Normal, origin);
        float dist = dot - axis.Width - radius;

        if (dist > 0)
          return -1;

        if (dist > penetration)
        {
          penetration = dist;
          found = index;
        }

        index++;
      }

      return found;
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Workhorse for circle-circle collisions, compares origin distance
    /// to the sum of the two circles' radii, returns a Manifold.
    /// </summary>
    /// 
    private static Manifold TestCircles(
      OriNocoWorld world,
      OriNocoCircle shapeA,
      OriNocoShape shapeB,
      Vector2 overrideBCenter, // For testing vertices in circles
      float overrideBRadius)
    {
      Vector2 r = overrideBCenter - shapeA.worldSpaceOrigin;
      float min = shapeA.radius + overrideBRadius;
      float distSq = r.sqrMagnitude;

      if (distSq >= min * min)
        return null;

      float dist = Mathf.Sqrt(distSq);
      float distInv = 1.0f / dist;

      Vector2 pos =
        shapeA.worldSpaceOrigin +
        (0.5f + distInv * (shapeA.radius - min / 2.0f)) * r;

      // Build the collision Manifold
      Manifold manifold = 
        world.AllocateManifold().Assign(world, shapeA, shapeB);
      manifold.AddContact(pos, distInv * r, dist - min);
      return manifold;
    }

    private static bool FindMinSepAxis(
      OriNocoPolygon poly1,
      OriNocoPolygon poly2,
      out Axis axis)
    {
      axis = new Axis(Vector2.zero, float.NegativeInfinity);

      for (int i = 0; i < poly1.countWorld; i++)
      {
        Axis a = poly1.worldAxes[i];
        float min = float.PositiveInfinity;
        for (int j = 0; j < poly2.countWorld; j++)
        {
          Vector2 v = poly2.worldVertices[j];
          min = Mathf.Min(min, Vector2.Dot(a.Normal, v));
        }
        min -= a.Width;

        if (min > 0)
          return false;
        if (min > axis.Width)
          axis = new Axis(a.Normal, min);
      }

      return true;
    }

    /// <summary>
    /// Add contacts for penetrating vertices. Note that this does not handle
    /// cases where an overlap was detected, but no vertices fall inside the
    /// opposing polygon (like a Star of David). For this we have a fallback.
    /// 
    /// See http://chipmunk-physics.googlecode.com/svn/trunk/src/cpCollision.c
    /// </summary>
    private static void FindVerts(
      OriNocoPolygon poly1,
      OriNocoPolygon poly2,
      Vector2 normal,
      float penetration,
      Manifold manifold)
    {
      bool found = false;

      for (int i = 0; i < poly1.countWorld; i++)
      {
        Vector2 vertex = poly1.worldVertices[i];
        if (poly2.ContainsPoint(vertex) == true)
        {
          if (manifold.AddContact(vertex, normal, penetration) == false)
            return;
          found = true;
        }
      }

      for (int i = 0; i < poly2.countWorld; i++)
      {
        Vector2 vertex = poly2.worldVertices[i];
        if (poly1.ContainsPoint(vertex) == true)
        {
          if (manifold.AddContact(vertex, normal, penetration) == false)
            return;
          found = true;
        }
      }

      // Fallback to check the degenerate "Star of David" case
      if (found == false)
        FindVertsFallback(poly1, poly2, normal, penetration, manifold);
    }

    /// <summary>
    /// A fallback for handling degenerate "Star of David" cases.
    /// </summary>
    private static void FindVertsFallback(
      OriNocoPolygon poly1,
      OriNocoPolygon poly2,
      Vector2 normal,
      float penetration,
      Manifold manifold)
    {
      for (int i = 0; i < poly1.countWorld; i++)
      {
        Vector2 vertex = poly1.worldVertices[i];
        if (poly2.ContainsPointPartial(vertex, normal) == true)
          if (manifold.AddContact(vertex, normal, penetration) == false)
            return;
      }

      for (int i = 0; i < poly2.countWorld; i++)
      {
        Vector2 vertex = poly2.worldVertices[i];
        if (poly1.ContainsPointPartial(vertex, -normal) == true)
          if (manifold.AddContact(vertex, normal, penetration) == false)
            return;
      }
    }
    #endregion
  }
}
