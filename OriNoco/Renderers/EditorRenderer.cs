using OriNoco.Rhine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static SDL2.SDL;

namespace OriNoco
{
    public class EditorRenderer : Game
    {
        public static EditorRenderer Instance;

        public Vector2 mousePos = new Vector2(0, 0);

        public Player player;
        public GUIText text;

        private Vector2 mousePosition;

        public TextureColored note;
        public Camera2D viewport;
        public float cameraOffset = 0.618f;

        public InputManager Input;

        public override void OnInitialized()
        {
            base.OnInitialized();
            Instance = this;

            viewport = new Camera2D(this);
            text = new GUIText(Renderer, GUI.font, "Hello, World!", new Color(1f, 1f, 1f));

            Input = new InputManager(ParentControl);
            player = new Player(this, Input);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            viewport.screenOffset = ViewportSize / 2;
            viewport.AdjustViewportToCameraOrthographicSize();
            // viewport.rotation += Time.deltaTime * 180f;
            mousePos = GetMousePosition();

            player.Update();
            viewport.offset = Vector2.Lerp(viewport.offset, player.position, Mathf.Abs((player.speed != 0 ? player.speed : 5) * cameraOffset * Time.deltaTime));

            Input.Update();
        }

        public override void OnRender()
        {
            SetColor(45, 45, 48);
            Clear();
            {
                SetColor(255, 255, 255);
                player.Render(viewport);
            }
            RenderPresent();
        }
    }

    public class Player
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

        public Player(Game game, InputManager input)
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
                if(TwoInputsAnyDown(Keys.Left, Keys.Up))
                {
                    direction = Direction.LeftUp;
                    rotation = 135f;
                    CreateLine();
                }
                else if (TwoInputsAnyDown(Keys.Left, Keys.Down))
                {
                    direction = Direction.LeftDown;
                    rotation = 45f;
                    CreateLine();
                }
                else if (TwoInputsAnyDown(Keys.Right, Keys.Up))
                {
                    direction = Direction.RightUp;
                    rotation = -135f;
                    CreateLine();
                }
                else if (TwoInputsAnyDown(Keys.Right, Keys.Down))
                {
                    direction = Direction.RightDown;
                    rotation = -45f;
                    CreateLine();
                }
                else if (Input.GetKeyDown(Keys.Left))
                {
                    direction = Direction.Left;
                    rotation = 90f;
                    CreateLine();
                }
                else if (Input.GetKeyDown(Keys.Right))
                {
                    direction = Direction.Right;
                    rotation = -90f;
                    CreateLine();
                }
                else if (Input.GetKeyDown(Keys.Down))
                {
                    direction = Direction.Down;
                    rotation = 180f;
                    CreateLine();
                }
                else if (Input.GetKeyDown(Keys.Up))
                {
                    direction = Direction.Up;
                    rotation = 0f;
                    CreateLine();
                }

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
                    Console.WriteLine("Up key detected");
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
            foreach(var line in lines)
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
