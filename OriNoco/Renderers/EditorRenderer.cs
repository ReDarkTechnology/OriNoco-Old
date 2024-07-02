using static SDL2.SDL;

namespace OriNoco
{
    public class EditorRenderer : Game
    {
        public static EditorRenderer Instance;

        public EditorPlayer player;
        public GUIText text;

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
            player = new EditorPlayer(this, Input);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            viewport.screenOffset = ViewportSize / 2;
            viewport.AdjustViewportToCameraOrthographicSize();

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
}
