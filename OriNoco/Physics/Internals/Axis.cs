using static SDL2.SDL;

namespace OriNoco
{
    /// <summary>
    /// The Axis data structure represents a "slab" between the given edge and
    /// a parallel edge drawn at the origin. The "width" value gives the width
    /// of that axis slab, defined as follows: For an edge AB with normal N, 
    /// this width w is given by Dot(A, N). If you take edge AB, and draw an 
    /// edge CD parallel to AB that intersects the origin, the width w is equal
    /// to the minimum distance between edges AB and CD.
    ///
    ///             |
    ///             |     C
    ///             |    /
    ///             |   /           A
    ///             |  /ヽ         /
    ///             | /   ヽ      /
    ///             |/    w ヽ   /
    ///  -----------+---------ヽ/----
    ///            /|          /
    ///           D |         /
    ///             |        /
    ///             |       B
    ///             |
    ///             
    /// </summary>
    internal struct Axis
    {
        internal SDL_FPoint Normal { get { return this.normal; } }
        internal float Width { get { return this.width; } }

        private readonly SDL_FPoint normal;
        private readonly float width;

        internal Axis(SDL_FPoint normal, float width)
        {
            this.normal = normal;
            this.width = width;
        }
    }
}