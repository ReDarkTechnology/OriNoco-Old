using System;
using static SDL2.SDL;

namespace OriNoco
{
	public static class Time
    {
        public static float time { get; private set; }
        public static float timeScale = 1;
        public static float deltaTime { get; private set; }
        public static float unscaledDeltaTime { get; private set; }
        public static float fixedDeltaTime { get; private set; }
        public static int framesSinceStart { get; private set; }

        private static ulong _now = SDL_GetPerformanceCounter();
        private static ulong _last = 0;

        public static void Update()
        {
            _last = _now;
            _now = SDL_GetPerformanceCounter();

            deltaTime = (_now - _last) * 1f / SDL_GetPerformanceFrequency();
            fixedDeltaTime = deltaTime;
            time += deltaTime;
            framesSinceStart++;
        }
    }
}