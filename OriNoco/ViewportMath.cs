using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace OriNoco
{
    public static class ViewportMath
    {
        public static Rect GetScreenRect(Rect objectRect, Vector2 screenSize)
        {
            return new Rect
            {
                x = objectRect.x + (screenSize.x / 2) - (objectRect.w / 2),
                y = objectRect.y + (screenSize.y / 2) - (objectRect.h / 2),
                w = objectRect.w,
                h = objectRect.h
            };
        }

        public static void GetScreenRect(ref Rect objectRect, Vector2 screenSize)
        {
            objectRect.x = objectRect.x + (screenSize.x / 2) - (objectRect.w / 2);
            objectRect.y = objectRect.y + (screenSize.y / 2) - (objectRect.h / 2);
        }

        public static Vector2 GetScreenPosition(Vector2 objectPoint, Vector2 screenSize)
        {
            objectPoint.x = objectPoint.x + (screenSize.x / 2);
            objectPoint.y = objectPoint.y + (screenSize.y / 2);
            return objectPoint;
        }
    }
}
