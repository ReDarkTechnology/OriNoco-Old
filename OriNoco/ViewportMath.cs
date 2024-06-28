using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace OriNoco
{
    public static class ViewportMath
    {
        public static SDL_FRect GetScreenRect(SDL_FRect objectRect, SDL_FPoint screenSize)
        {
            return new SDL_FRect
            {
                x = objectRect.x + (screenSize.x / 2) - (objectRect.w / 2),
                y = objectRect.y + (screenSize.y / 2) - (objectRect.h / 2),
                w = objectRect.w,
                h = objectRect.h
            };
        }

        public static void GetScreenRect(ref SDL_FRect objectRect, SDL_FPoint screenSize)
        {
            objectRect.x = objectRect.x + (screenSize.x / 2) - (objectRect.w / 2);
            objectRect.y = objectRect.y + (screenSize.y / 2) - (objectRect.h / 2);
        }

        public static SDL_FPoint GetScreenPosition(SDL_FPoint objectPoint, SDL_FPoint screenSize)
        {
            objectPoint.x = objectPoint.x + (screenSize.x / 2);
            objectPoint.y = objectPoint.y + (screenSize.y / 2);
            return objectPoint;
        }
    }
}
