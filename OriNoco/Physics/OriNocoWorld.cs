using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace OriNoco
{
    public partial class OriNocoWorld
    {
        #region Helper Filters
        public static bool FilterNone(OriNocoBody body)
        {
            return true;
        }

        public static bool FilterAll(OriNocoBody body)
        {
            return false;
        }

        public static bool FilterStatic(OriNocoBody body)
        {
            return (body.IsStatic == false);
        }

        public static bool FilterDynamic(OriNocoBody body)
        {
            return body.IsStatic;
        }

        public static VoltBodyFilter FilterExcept(OriNocoBody exception)
        {
            return ((body) => body != exception);
        }
        #endregion

        /// <summary>
        /// Fixed update delta time for body integration. 
        /// Defaults to Config.DEFAULT_DELTA_TIME.
        /// </summary>
        public float DeltaTime { get; set; }

        /// <summary>
        /// Number of iterations when updating the world.
        /// Defaults to Config.DEFAULT_ITERATION_COUNT.
        /// </summary>
        public int IterationCount { get; set; }

        /// <summary>
        /// How many frames of history this world is recording.
        /// </summary>
        public int HistoryLength { get; private set; }

        public IEnumerable<OriNocoBody> Bodies
        {
            get
            {
                for (int i = 0; i < this.bodies.Count; i++)
                    yield return this.bodies[i];
            }
        }

        internal float Elasticity { get; private set; }
        internal float Damping { get; private set; }

        private CheapList<OriNocoBody> bodies;
        private List<Manifold> manifolds;

        private IBroadPhase dynamicBroadphase;
        private IBroadPhase staticBroadphase;

        private OriNocoBuffer<OriNocoBody> reusableBuffer;
        private OriNocoBuffer<OriNocoBody> reusableOutput;

        // Each World instance should own its own object pools, in case
        // you want to run multiple World instances simultaneously.
        private IOriNocoPool<OriNocoBody> bodyPool;
        private IOriNocoPool<OriNocoShape> circlePool;
        private IOriNocoPool<OriNocoShape> polygonPool;

        private IOriNocoPool<Contact> contactPool;
        private IOriNocoPool<Manifold> manifoldPool;
        private IOriNocoPool<HistoryBuffer> historyPool;

        public OriNocoWorld(
          int historyLength = 0,
          float damping = OriNocoConfig.DEFAULT_DAMPING)
        {
            this.HistoryLength = historyLength;
            this.Damping = damping;

            this.IterationCount = OriNocoConfig.DEFAULT_ITERATION_COUNT;
            this.DeltaTime = OriNocoConfig.DEFAULT_DELTA_TIME;

            this.bodies = new CheapList<OriNocoBody>();
            this.manifolds = new List<Manifold>();

            this.dynamicBroadphase = new NaiveBroadphase();
            this.staticBroadphase = new TreeBroadphase();

            this.reusableBuffer = new OriNocoBuffer<OriNocoBody>();
            this.reusableOutput = new OriNocoBuffer<OriNocoBody>();

            this.bodyPool = new OriNocoPool<OriNocoBody>();
            this.circlePool = new VoltPool<OriNocoShape, OriNocoCircle>();
            this.polygonPool = new VoltPool<OriNocoShape, OriNocoPolygon>();

            this.contactPool = new OriNocoPool<Contact>();
            this.manifoldPool = new OriNocoPool<Manifold>();
            this.historyPool = new OriNocoPool<HistoryBuffer>();
        }

        /// <summary>
        /// Creates a new polygon shape from world-space vertices.
        /// </summary>
        public OriNocoPolygon CreatePolygonWorldSpace(
          SDL_FPoint[] worldVertices,
          float density = OriNocoConfig.DEFAULT_DENSITY,
          float friction = OriNocoConfig.DEFAULT_FRICTION,
          float restitution = OriNocoConfig.DEFAULT_RESTITUTION)
        {
            OriNocoPolygon polygon = (OriNocoPolygon)this.polygonPool.Allocate();
            polygon.InitializeFromWorldVertices(
              worldVertices,
              density,
              friction,
              restitution);
            return polygon;
        }

        /// <summary>
        /// Creates a new polygon shape from body-space vertices.
        /// </summary>
        public OriNocoPolygon CreatePolygonBodySpace(
          SDL_FPoint[] bodyVertices,
          float density = OriNocoConfig.DEFAULT_DENSITY,
          float friction = OriNocoConfig.DEFAULT_FRICTION,
          float restitution = OriNocoConfig.DEFAULT_RESTITUTION)
        {
            OriNocoPolygon polygon = (OriNocoPolygon)this.polygonPool.Allocate();
            polygon.InitializeFromBodyVertices(
              bodyVertices,
              density,
              friction,
              restitution);
            return polygon;
        }

        /// <summary>
        /// Creates a new circle shape from a world-space origin.
        /// </summary>
        public OriNocoCircle CreateCircleWorldSpace(
          SDL_FPoint worldSpaceOrigin,
          float radius,
          float density = OriNocoConfig.DEFAULT_DENSITY,
          float friction = OriNocoConfig.DEFAULT_FRICTION,
          float restitution = OriNocoConfig.DEFAULT_RESTITUTION)
        {
            OriNocoCircle circle = (OriNocoCircle)this.circlePool.Allocate();
            circle.InitializeFromWorldSpace(
              worldSpaceOrigin,
              radius,
              density,
              friction,
              restitution);
            return circle;
        }

        /// <summary>
        /// Creates a new static body and adds it to the world.
        /// </summary>
        public OriNocoBody CreateStaticBody(
          SDL_FPoint position,
          float radians,
          params OriNocoShape[] shapesToAdd)
        {
            OriNocoBody body = this.bodyPool.Allocate();
            body.InitializeStatic(position, radians, shapesToAdd);
            this.AddBodyInternal(body);
            return body;
        }

        /// <summary>
        /// Creates a new dynamic body and adds it to the world.
        /// </summary>
        public OriNocoBody CreateDynamicBody(
          SDL_FPoint position,
          float radians,
          params OriNocoShape[] shapesToAdd)
        {
            OriNocoBody body = this.bodyPool.Allocate();
            body.InitializeDynamic(position, radians, shapesToAdd);
            this.AddBodyInternal(body);
            return body;
        }

        /// <summary>
        /// Adds a body to the world. Used for reintroducing bodies that 
        /// have been removed. For new bodies, use CreateBody.
        /// </summary>
        public void AddBody(
          OriNocoBody body,
          SDL_FPoint position,
          float radians)
        {
#if DEBUG
            OriNocoDebug.Assert(body.IsInitialized);
#endif
            OriNocoDebug.Assert(body.World == null);
            this.AddBodyInternal(body);
            body.Set(position, radians);
        }

        /// <summary>
        /// Removes a body from the world. The body will be partially reset so it
        /// can be added later. The pointer is still valid and the body can be
        /// returned to the world using AddBody.
        /// </summary>
        public void RemoveBody(OriNocoBody body)
        {
            OriNocoDebug.Assert(body.World == this);

            body.PartialReset();

            this.RemoveBodyInternal(body);
        }

        /// <summary>
        /// Removes a body from the world and deallocates it. The pointer is
        /// invalid after this point.
        /// </summary>
        public void DestroyBody(OriNocoBody body)
        {
            OriNocoDebug.Assert(body.World == this);

            body.FreeShapes();

            this.RemoveBodyInternal(body);
            this.FreeBody(body);
        }

        /// <summary>
        /// Ticks the world, updating all dynamic bodies and resolving collisions.
        /// If a frame number is provided, all dynamic bodies will store their
        /// state for that frame for later testing.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < this.bodies.Count; i++)
            {
                OriNocoBody body = this.bodies[i];
                if (body.IsStatic == false)
                {
                    body.Update();
                    this.dynamicBroadphase.UpdateBody(body);
                }
            }

            this.BroadPhase();

            this.UpdateCollision();
            this.FreeManifolds();
        }

        /// <summary>
        /// Updates a single body, resolving only collisions with that body.
        /// If a frame number is provided, all dynamic bodies will store their
        /// state for that frame for later testing.
        /// 
        /// Note: This function is best used with dynamic collisions disabled, 
        /// otherwise you might get symmetric duplicates on collisions.
        /// </summary>
        public void Update(OriNocoBody body, bool collideDynamic = false)
        {
            if (body.IsStatic)
            {
                OriNocoDebug.LogWarning("Updating static body, doing nothing");
                return;
            }

            body.Update();
            this.dynamicBroadphase.UpdateBody(body);
            this.BroadPhase(body, collideDynamic);

            this.UpdateCollision();
            this.FreeManifolds();
        }

        /// <summary>
        /// Finds all bodies containing a given point.
        /// 
        /// Subsequent calls to other Query functions (Point, Circle, Bounds) will
        /// invalidate the resulting enumeration from this function.
        /// </summary>
        public OriNocoBuffer<OriNocoBody> QueryPoint(
          SDL_FPoint point,
          VoltBodyFilter filter = null,
          int ticksBehind = 0)
        {
            if (ticksBehind < 0)
                throw new ArgumentOutOfRangeException("ticksBehind");

            this.reusableBuffer.Clear();
            this.staticBroadphase.QueryPoint(point, this.reusableBuffer);
            this.dynamicBroadphase.QueryPoint(point, this.reusableBuffer);

            this.reusableOutput.Clear();
            for (int i = 0; i < this.reusableBuffer.Count; i++)
            {
                OriNocoBody body = this.reusableBuffer[i];
                if (OriNocoBody.Filter(body, filter))
                    if (body.QueryPoint(point, ticksBehind))
                        this.reusableOutput.Add(body);
            }
            return this.reusableOutput;
        }

        /// <summary>
        /// Finds all bodies intersecting with a given circle.
        /// 
        /// Subsequent calls to other Query functions (Point, Circle, Bounds) will
        /// invalidate the resulting enumeration from this function.
        /// </summary>
        public OriNocoBuffer<OriNocoBody> QueryCircle(
          SDL_FPoint origin,
          float radius,
          VoltBodyFilter filter = null,
          int ticksBehind = 0)
        {
            if (ticksBehind < 0)
                throw new ArgumentOutOfRangeException("ticksBehind");

            this.reusableBuffer.Clear();
            this.staticBroadphase.QueryCircle(origin, radius, this.reusableBuffer);
            this.dynamicBroadphase.QueryCircle(origin, radius, this.reusableBuffer);

            this.reusableOutput.Clear();
            for (int i = 0; i < this.reusableBuffer.Count; i++)
            {
                OriNocoBody body = this.reusableBuffer[i];
                if (OriNocoBody.Filter(body, filter))
                    if (body.QueryCircle(origin, radius, ticksBehind))
                        this.reusableOutput.Add(body);
            }

            return this.reusableOutput;
        }

        /// <summary>
        /// Performs a raycast on all world bodies.
        /// </summary>
        public bool RayCast(
          ref OriNocoRayCast ray,
          ref OriNocoRayResult result,
          VoltBodyFilter filter = null,
          int ticksBehind = 0)
        {
            if (ticksBehind < 0)
                throw new ArgumentOutOfRangeException("ticksBehind");

            this.reusableBuffer.Clear();
            this.staticBroadphase.RayCast(ref ray, this.reusableBuffer);
            this.dynamicBroadphase.RayCast(ref ray, this.reusableBuffer);

            for (int i = 0; i < this.reusableBuffer.Count; i++)
            {
                OriNocoBody body = this.reusableBuffer[i];
                if (OriNocoBody.Filter(body, filter))
                {
                    body.RayCast(ref ray, ref result, ticksBehind);
                    if (result.IsContained)
                        return true;
                }
            }

            return result.IsValid;
        }

        /// <summary>
        /// Performs a circle cast on all world bodies.
        /// </summary>
        public bool CircleCast(
          ref OriNocoRayCast ray,
          float radius,
          ref OriNocoRayResult result,
          VoltBodyFilter filter = null,
          int ticksBehind = 0)
        {
            if (ticksBehind < 0)
                throw new ArgumentOutOfRangeException("ticksBehind");

            this.reusableBuffer.Clear();
            this.staticBroadphase.CircleCast(ref ray, radius, this.reusableBuffer);
            this.dynamicBroadphase.CircleCast(ref ray, radius, this.reusableBuffer);

            for (int i = 0; i < this.reusableBuffer.Count; i++)
            {
                OriNocoBody body = this.reusableBuffer[i];
                if (OriNocoBody.Filter(body, filter))
                {
                    body.CircleCast(ref ray, radius, ref result, ticksBehind);
                    if (result.IsContained)
                        return true;
                }
            }
            return result.IsValid;
        }

        #region Internals
        private void AddBodyInternal(OriNocoBody body)
        {
            this.bodies.Add(body);
            if (body.IsStatic)
                this.staticBroadphase.AddBody(body);
            else
                this.dynamicBroadphase.AddBody(body);

            body.AssignWorld(this);
            if ((this.HistoryLength > 0) && (body.IsStatic == false))
                body.AssignHistory(this.AllocateHistory());
        }

        private void RemoveBodyInternal(OriNocoBody body)
        {
            this.bodies.Remove(body);
            if (body.IsStatic)
                this.staticBroadphase.RemoveBody(body);
            else
                this.dynamicBroadphase.RemoveBody(body);

            body.FreeHistory();
            body.AssignWorld(null);
        }

        /// <summary>
        /// Identifies collisions for all bodies, ignoring symmetrical duplicates.
        /// </summary>
        private void BroadPhase()
        {
            for (int i = 0; i < this.bodies.Count; i++)
            {
                OriNocoBody query = this.bodies[i];
                if (query.IsStatic)
                    continue;

                this.reusableBuffer.Clear();
                this.staticBroadphase.QueryOverlap(query.AABB, this.reusableBuffer);

                // HACK: Don't use dynamic broadphase for global updates for this.
                // It's faster if we do it manually because we can triangularize.
                for (int j = i + 1; j < this.bodies.Count; j++)
                    if (this.bodies[j].IsStatic == false)
                        this.reusableBuffer.Add(this.bodies[j]);

                this.TestBuffer(query);
            }
        }

        /// <summary>
        /// Identifies collisions for a single body. Does not keep track of 
        /// symmetrical duplicates (they could be counted twice).
        /// </summary>
        private void BroadPhase(OriNocoBody query, bool collideDynamic = false)
        {
            OriNocoDebug.Assert(query.IsStatic == false);

            this.reusableBuffer.Clear();
            this.staticBroadphase.QueryOverlap(query.AABB, this.reusableBuffer);
            if (collideDynamic)
                this.dynamicBroadphase.QueryOverlap(query.AABB, this.reusableBuffer);

            this.TestBuffer(query);
        }

        private void TestBuffer(OriNocoBody query)
        {
            for (int i = 0; i < this.reusableBuffer.Count; i++)
            {
                OriNocoBody test = this.reusableBuffer[i];
                bool canCollide =
                  query.CanCollide(test) &&
                  test.CanCollide(query) &&
                  query.AABB.Intersect(test.AABB);

                if (canCollide)
                    for (int i_q = 0; i_q < query.shapeCount; i_q++)
                        for (int j_t = 0; j_t < test.shapeCount; j_t++)
                            this.NarrowPhase(query.shapes[i_q], test.shapes[j_t]);
            }
        }

        /// <summary>
        /// Creates a manifold for two shapes if they collide.
        /// </summary>
        private void NarrowPhase(
          OriNocoShape sa,
          OriNocoShape sb)
        {
            if (sa.AABB.Intersect(sb.AABB) == false)
                return;

            OriNocoShape.OrderShapes(ref sa, ref sb);
            Manifold manifold = Collision.Dispatch(this, sa, sb);
            if (manifold != null)
                this.manifolds.Add(manifold);
        }

        private void UpdateCollision()
        {
            for (int i = 0; i < this.manifolds.Count; i++)
                this.manifolds[i].PreStep();

            this.Elasticity = 1.0f;
            for (int j = 0; j < this.IterationCount * 1 / 3; j++)
                for (int i = 0; i < this.manifolds.Count; i++)
                    this.manifolds[i].Solve();

            for (int i = 0; i < this.manifolds.Count; i++)
                this.manifolds[i].SolveCached();

            this.Elasticity = 0.0f;
            for (int j = 0; j < this.IterationCount * 2 / 3; j++)
                for (int i = 0; i < this.manifolds.Count; i++)
                    this.manifolds[i].Solve();
        }

        #region Pooling
        internal Contact AllocateContact()
        {
            return this.contactPool.Allocate();
        }

        internal Manifold AllocateManifold()
        {
            return this.manifoldPool.Allocate();
        }

        private HistoryBuffer AllocateHistory()
        {
            HistoryBuffer history = this.historyPool.Allocate();
            history.Initialize(this.HistoryLength);
            return history;
        }

        private void FreeBody(OriNocoBody body)
        {
            this.bodyPool.Deallocate(body);
        }

        private void FreeManifolds()
        {
            for (int i = 0; i < this.manifolds.Count; i++)
                this.manifoldPool.Deallocate(this.manifolds[i]);
            this.manifolds.Clear();
        }

        internal void FreeContacts(IList<Contact> contacts)
        {
            for (int i = 0; i < contacts.Count; i++)
                this.contactPool.Deallocate(contacts[i]);
        }

        internal void FreeHistory(HistoryBuffer history)
        {
            this.historyPool.Deallocate(history);
        }

        internal void FreeShape(OriNocoShape shape)
        {
            switch (shape.Type)
            {
                case OriNocoShape.ShapeType.Circle:
                    this.circlePool.Deallocate(shape);
                    break;

                case OriNocoShape.ShapeType.Polygon:
                    this.polygonPool.Deallocate(shape);
                    break;

                default:
                    OriNocoDebug.LogError("Unknown shape for deallocation");
                    break;
            }
        }

        private OriNocoCircle CreateCircle()
        {
            return new OriNocoCircle();
        }

        private OriNocoPolygon CreatePolygon()
        {
            return new OriNocoPolygon();
        }
        #endregion
        #endregion
    }
}