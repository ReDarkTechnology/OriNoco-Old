﻿namespace OriNoco
{
    public static class OriNocoConfig
    {
        public static float ResolveSlop = 0.01f;
        public static float ResolveRate = 0.1f;
        public static float AreaMassRatio = 0.01f;

        // Defaults
        public const float DEFAULT_DENSITY = 1.0f;
        public const float DEFAULT_RESTITUTION = 0.5f;
        public const float DEFAULT_FRICTION = 0.8f;

        internal const float DEFAULT_DELTA_TIME = 0.02f;
        internal const float DEFAULT_DAMPING = 0.999f;
        internal const int DEFAULT_ITERATION_COUNT = 20;

        // AABB extension for the dynamic tree
        internal const float AABB_EXTENSION = 0.2f;

        // Maximum contacts for collision resolution.
        internal const int MAX_CONTACTS = 3;

        // Used for initializing timesteps
        internal const int INVALID_TIME = -1;

        // The minimum mass a dynamic object can have before it is
        // converted to a static object
        internal const float MINIMUM_DYNAMIC_MASS = 0.00001f;
    }
}