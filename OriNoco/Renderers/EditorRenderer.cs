using System.Windows.Forms;
using static SDL2.SDL;

namespace OriNoco
{
    public class EditorRenderer : Game
    {
        public static EditorRenderer Instance;

        public Vector2 mousePos = new Vector2(0, 0);

        public Rect playerRect = new Rect(Vector2.zero, Vector2.zero);
        public Color32 playerColor = new Color32(0, 173, 91);

        private Rect playerViewportRect = new Rect();

        public GUIText text;

        private Vector2 mousePosition;
        public TextureRenderer note;
        public Camera2D viewport;

        public override void OnInitialized()
        {
            base.OnInitialized();
            Instance = this;

            viewport = new Camera2D(this);
            text = new GUIText(Renderer, GUI.font, "Hello, World!", new Color(1f, 1f, 1f));

            note = new TextureRenderer(Renderer, "Images/Note.png");
            playerRect = new Rect(Vector2.zero, new Vector2(note.w, note.h) * 0.2f / 100f);

            ParentControl.MouseDown += OnMouseDown;
            ParentControl.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {

        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            viewport.screenOffset = ViewportSize / 2;
            viewport.AdjustViewportToCameraOrthographicSize();
            // viewport.rotation += Time.deltaTime * 180f;
            mousePos = GetMousePosition();

            playerViewportRect = viewport.ToScreenRect(playerRect);
        }

        public override void OnRender()
        {
            SetColor(45, 45, 48);
            Clear();
            {
                SetColor(255, 255, 255);

                note.color = playerColor;
                note.Draw(playerViewportRect, viewport.rotation);
            }
            RenderPresent();
        }
    }
}
