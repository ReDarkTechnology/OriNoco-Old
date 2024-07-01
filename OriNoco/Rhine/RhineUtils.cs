using static SDL2.SDL;

namespace OriNoco.Rhine
{
    public static class RhineUtils
    {
        const float sqrt2Div2 = 0.70710678118654752440084436210485f;
        public static readonly Vector2[] moveVectors = {
            new(1f, 0),
            new(0, 1f),
            new(-1f, 0),
            new(0, -1f),
            new(sqrt2Div2, sqrt2Div2),
            new(sqrt2Div2, -sqrt2Div2),
            new(-sqrt2Div2, sqrt2Div2),
            new(-sqrt2Div2, -sqrt2Div2),
        };
    }
}
