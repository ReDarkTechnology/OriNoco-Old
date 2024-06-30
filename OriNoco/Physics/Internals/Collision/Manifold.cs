using static SDL2.SDL;

namespace OriNoco
{
  internal sealed class Manifold
    : IOriNocoPoolable<Manifold>
  {
    #region Interface
    IOriNocoPool<Manifold> IOriNocoPoolable<Manifold>.Pool { get; set; }
    void IOriNocoPoolable<Manifold>.Reset() { this.Reset(); }
    #endregion

    internal OriNocoShape ShapeA { get; private set; }
    internal OriNocoShape ShapeB { get; private set; }
    internal float Restitution { get; private set; }
    internal float Friction { get; private set; }

    private readonly Contact[] contacts;
    private int used = 0;
    private OriNocoWorld world;

    public Manifold()
    {
      this.contacts = new Contact[OriNocoConfig.MAX_CONTACTS];
      this.used = 0;
      this.Reset();
    }

    internal Manifold Assign(
      OriNocoWorld world,
      OriNocoShape shapeA,
      OriNocoShape shapeB)
    {
      this.world = world;
      this.ShapeA = shapeA;
      this.ShapeB = shapeB;

      this.Restitution = Mathf.Sqrt(shapeA.Restitution * shapeB.Restitution);
      this.Friction = Mathf.Sqrt(shapeA.Friction * shapeB.Friction);
      this.used = 0;

      return this;
    }

    internal bool AddContact(
      Vector2 position,
      Vector2 normal,
      float penetration)
    {
      if (this.used >= OriNocoConfig.MAX_CONTACTS)
        return false;

      this.contacts[this.used] =
        this.world.AllocateContact().Assign(
          position, 
          normal, 
          penetration);
      this.used++;

      return true;
    }

    internal void PreStep()
    {
      for (int i = 0; i < this.used; i++)
        this.contacts[i].PreStep(this);
    }

    internal void Solve()
    {
      for (int i = 0; i < this.used; i++)
        this.contacts[i].Solve(this);
    }

    internal void SolveCached()
    {
      for (int i = 0; i < this.used; i++)
        this.contacts[i].SolveCached(this);
    }

    private void ClearContacts()
    {
      for (int i = 0; i < this.used; i++)
        OriNocoPool.Free(this.contacts[i]);
      this.used = 0;
    }

    private void Reset()
    {
      this.ShapeA = null;
      this.ShapeB = null;
      this.Restitution = 0.0f;
      this.Friction = 0.0f;

      this.ClearContacts();
      this.world = null;
    }
  }
}