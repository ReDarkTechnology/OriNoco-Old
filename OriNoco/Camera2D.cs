using System;
using static SDL2.SDL;

namespace OriNoco
{
    public class Camera2D : Viewport
    {
        public static float verticalScreenToOrthroValue = 0.5f;
        public Game game { get; set; }
        private float _orthSize = 5f;
        public float orthographicSize {
            get => _orthSize;
            set {
                _orthSize = value;
                AdjustViewportToCameraOrthographicSize(value);
            } 
        }

        public Camera2D(Game game, float orthographicSize = 5f, Vector2 offset = default, float rotation = 0)
        {
            this.game = game;
            screenOffset = game.ViewportSize / 2;
            this.orthographicSize = orthographicSize;
            this.offset = offset;
            this.rotation = rotation;
        }

        public void AdjustViewportToCameraOrthographicSize() => AdjustViewportToCameraOrthographicSize(_orthSize);
        public void AdjustViewportToCameraOrthographicSize(float orthographicSize)
        {
            pixelScale = game.ViewportSize.y / orthographicSize * verticalScreenToOrthroValue;
        }
    }
}
