using static SDL2.SDL;

namespace OriNoco
{
  internal sealed class Contact 
    : IOriNocoPoolable<Contact>
  {
    #region Interface
    IOriNocoPool<Contact> IOriNocoPoolable<Contact>.Pool { get; set; }
    void IOriNocoPoolable<Contact>.Reset() { this.Reset(); }
    #endregion

    #region Static Methods
    private static float BiasDist(float dist)
    {
      return OriNocoConfig.ResolveRate * Mathf.Min(0, dist + OriNocoConfig.ResolveSlop);
    }
    #endregion

    private SDL_FPoint position;
    private SDL_FPoint normal;
    private float penetration;

    private SDL_FPoint toA;
    private SDL_FPoint toB;
    private SDL_FPoint toALeft;
    private SDL_FPoint toBLeft;

    private float nMass;
    private float tMass;
    private float restitution;
    private float bias;
    private float jBias;

    private float cachedNormalImpulse;
    private float cachedTangentImpulse;

    public Contact()
    {
      this.Reset();
    }

    internal Contact Assign(
      SDL_FPoint position,
      SDL_FPoint normal,
      float penetration)
    {
      this.Reset();

      this.position = position;
      this.normal = normal;
      this.penetration = penetration;

      return this;
    }

    internal void PreStep(Manifold manifold)
    {
      OriNocoBody bodyA = manifold.ShapeA.Body;
      OriNocoBody bodyB = manifold.ShapeB.Body;

      this.toA = this.position - bodyA.Position;
      this.toB = this.position - bodyB.Position;
      this.toALeft = this.toA.Left();
      this.toBLeft = this.toB.Left();

      this.nMass = 1.0f / this.KScalar(bodyA, bodyB, this.normal);
      this.tMass = 1.0f / this.KScalar(bodyA, bodyB, this.normal.Left());

      this.bias = Contact.BiasDist(penetration);
      this.jBias = 0;
      this.restitution =
        manifold.Restitution *
        SDL_FPoint.Dot(
          this.normal,
          this.RelativeVelocity(bodyA, bodyB));
    }

    internal void SolveCached(Manifold manifold)
    {
      this.ApplyContactImpulse(
        manifold.ShapeA.Body,
        manifold.ShapeB.Body,
        this.cachedNormalImpulse,
        this.cachedTangentImpulse);
    }

    internal void Solve(Manifold manifold)
    {
      OriNocoBody bodyA = manifold.ShapeA.Body;
      OriNocoBody bodyB = manifold.ShapeB.Body;
      float elasticity = bodyA.World.Elasticity;

      // Calculate relative bias velocity
      SDL_FPoint vb1 = bodyA.BiasVelocity + (bodyA.BiasRotation * this.toALeft);
      SDL_FPoint vb2 = bodyB.BiasVelocity + (bodyB.BiasRotation * this.toBLeft);
      float vbn = SDL_FPoint.Dot((vb1 - vb2), this.normal);

      // Calculate and clamp the bias impulse
      float jbn = this.nMass * (vbn - this.bias);
      jbn = Mathf.Max(-this.jBias, jbn);
      this.jBias += jbn;

      // Apply the bias impulse
      this.ApplyNormalBiasImpulse(bodyA, bodyB, jbn);

      // Calculate relative velocity
      SDL_FPoint vr = this.RelativeVelocity(bodyA, bodyB);
      float vrn = SDL_FPoint.Dot(vr, this.normal);

      // Calculate and clamp the normal impulse
      float jn = nMass * (vrn + (this.restitution * elasticity));
      jn = Mathf.Max(-this.cachedNormalImpulse, jn);
      this.cachedNormalImpulse += jn;

      // Calculate the relative tangent velocity
      float vrt = SDL_FPoint.Dot(vr, this.normal.Left());

      // Calculate and clamp the friction impulse
      float jtMax = manifold.Friction * this.cachedNormalImpulse;
      float jt = vrt * tMass;
      float result = Mathf.Clamp(this.cachedTangentImpulse + jt, -jtMax, jtMax);
      jt = result - this.cachedTangentImpulse;
      this.cachedTangentImpulse = result;

      // Apply the normal and tangent impulse
      this.ApplyContactImpulse(bodyA, bodyB, jn, jt);
    }

    #region Internals
    private void Reset()
    {
      this.position = SDL_FPoint.zero;
      this.normal = SDL_FPoint.zero;
      this.penetration = 0.0f;

      this.toA = SDL_FPoint.zero;
      this.toB = SDL_FPoint.zero;
      this.toALeft = SDL_FPoint.zero;
      this.toBLeft = SDL_FPoint.zero;

      this.nMass = 0.0f;
      this.tMass = 0.0f;
      this.restitution = 0.0f;
      this.bias = 0.0f;
      this.jBias = 0.0f;

      this.cachedNormalImpulse = 0.0f;
      this.cachedTangentImpulse = 0.0f;
    }

    private float KScalar(
      OriNocoBody bodyA,
      OriNocoBody bodyB,
      SDL_FPoint normal)
    {
      float massSum = bodyA.InvMass + bodyB.InvMass;
      float r1cnSqr = OriNocoMath.Square(OriNocoMath.Cross(this.toA, normal));
      float r2cnSqr = OriNocoMath.Square(OriNocoMath.Cross(this.toB, normal));
      return
        massSum +
        bodyA.InvInertia * r1cnSqr +
        bodyB.InvInertia * r2cnSqr;
    }

    private SDL_FPoint RelativeVelocity(OriNocoBody bodyA, OriNocoBody bodyB)
    {
      return
        (bodyA.AngularVelocity * this.toALeft + bodyA.LinearVelocity) -
        (bodyB.AngularVelocity * this.toBLeft + bodyB.LinearVelocity);
    }

    private void ApplyNormalBiasImpulse(
      OriNocoBody bodyA,
      OriNocoBody bodyB,
      float normalBiasImpulse)
    {
      SDL_FPoint impulse = normalBiasImpulse * this.normal;
      bodyA.ApplyBias(-impulse, this.toA);
      bodyB.ApplyBias(impulse, this.toB);
    }

    private void ApplyContactImpulse(
      OriNocoBody bodyA,
      OriNocoBody bodyB,
      float normalImpulseMagnitude,
      float tangentImpulseMagnitude)
    {
      SDL_FPoint impulseWorld =
        new SDL_FPoint(normalImpulseMagnitude, tangentImpulseMagnitude);
      SDL_FPoint impulse = impulseWorld.Rotate(this.normal);

      bodyA.ApplyImpulse(-impulse, this.toA);
      bodyB.ApplyImpulse(impulse, this.toB);
    }
    #endregion
  }
}