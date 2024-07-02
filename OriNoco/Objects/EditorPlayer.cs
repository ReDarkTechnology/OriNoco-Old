using System;
using System.Windows.Forms;
using System.Collections.Generic;

using static SDL2.SDL;
using OriNoco.Rhine;

namespace OriNoco
{
    public class EditorPlayer
    {
        public Game game;
        public TextureRenderer renderer;
        public TextureRenderer lineRenderer;
        public Direction direction = Direction.Up;

        public Vector2 position;
        public bool isStarted;
        public float rotation;

        public float speed = 6;

        public InputManager Input;
        private Rect destRect;
        private Rect currentRect;

        public List<LineInstance> lines = new List<LineInstance>();
        public int lineCount = -1;

        public EditorPlayer(Game game, InputManager input)
        {
            this.game = game;
            Input = input;

            renderer = new TextureRenderer(game.Renderer, "Images/Note.png");
            renderer.color = new Color32(0, 173, 91);

            lineRenderer = new TextureRenderer(game.Renderer, "Images/Box.png");

            currentRect = new Rect(position, renderer.size / 100f * 0.2f);
        }

        public void Update()
        {
            if (isStarted)
            {
                Vector2 movementDirection = RhineUtils.moveVectors[(int)direction] * Time.deltaTime * speed;
                position += movementDirection;

                var line = lines[lineCount];

                line.scale += Time.deltaTime * speed;
                line.position += movementDirection / 2f;

                currentRect.center = position;
            }
            else
            {
                if (Input.GetKeyDown(Keys.Up))
                {
                    CreateLine();
                    isStarted = true;
                    direction = Direction.Up;
                    rotation = 0;
                }
            }
        }

        public void CreateLine()
        {
            lines.Add(new LineInstance
            {
                position = position,
                rotation = rotation
            });
            lineCount++;
        }

        private bool TwoInputsAnyDown(Keys a, Keys b) => (Input.GetKeyDown(a) && Input.GetKey(b)) || (Input.GetKey(a) && Input.GetKeyDown(b));
        private bool TwoInputsHeld(Keys a, Keys b) => (Input.GetKey(a) && Input.GetKey(b));

        public void Render(Viewport viewport)
        {
            Rect destLine = new Rect();
            foreach (var line in lines)
            {
                destLine = viewport.ToScreenRect(line.rect);
                lineRenderer.Draw(destLine, viewport.rotation + line.rotation);
            }

            destRect = viewport.ToScreenRect(currentRect);
            renderer.Draw(destRect, viewport.rotation + rotation);
        }

        public class LineInstance
        {
            private Vector2 _position;
            public Vector2 position
            {
                get => _position;
                set
                {
                    _position = value;
                    rect.center = value;
                }
            }

            public float scale
            {
                get => rect.h;
                set => rect.h = value;
            }

            public float rotation;

            public Rect rect = new Rect(new Vector2(0, 0), new Vector2(0.1f, 0.1f));
        }
    }
}
