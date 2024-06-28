using System.IO;
using System.Windows.Forms;
using static SDL2.SDL;

namespace OriNoco
{
    public class ChartView : Game
    {
        public static ChartView Instance;

        public SDL_Color boxColor = new SDL_Color(255, 0, 0, 255);
        public SDL_FRect boxRect = new SDL_FRect(new SDL_FPoint(0, -50), new SDL_FPoint(50, 50));
        public SDL_FPoint mousePos = new SDL_FPoint(0, 0);
        public SDL_FRect spriteRect;
        public SDL_Rect sourceRect;

        private SDL_FRect boxViewportRect = new SDL_FRect();
        private SDL_FRect spriteViewportRect = new SDL_FRect();

        public GUIText text;

        bool isMouseDown;
        bool insideBox = false;
        bool big = false;
        SDL_FPoint mousePosition;
        public TextureRenderer yotsuchi;
        public Viewport viewport = new Viewport(1f, new SDL_FPoint(), new SDL_FPoint(), 0);
        int u = 0;

        public override void OnInitialized()
        {
            base.OnInitialized();
            Instance = this;
            GUI.LoadFont("times.ttf", 32);
            text = new GUIText(Renderer, GUI.font, "Hello, World!", new SDL_FColor(1f, 1f, 1f, 1f));

            yotsuchi = new TextureRenderer(Renderer, "Images/Yotsuchi.png");
            yotsuchi.color = new SDL_Color(0, 255, 0, 255);
            spriteRect = new SDL_FRect(new SDL_FPoint(0, -50), new SDL_FPoint(100, 100));

            ParentControl.MouseDown += OnMouseDown;
            ParentControl.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            if (insideBox)
            {
                if (big)
                    boxRect = new SDL_FRect(new SDL_FPoint(0, -50), new SDL_FPoint(50, 50));
                else
                    boxRect = new SDL_FRect(new SDL_FPoint(0, -50), new SDL_FPoint(100, 100));
                big = !big;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            viewport.screenOffset = ViewportSize / 2;
            viewport.rotation += Time.deltaTime * 180f;
            mousePos = GetMousePosition();
            boxViewportRect = viewport.ToScreenRect(boxRect);
            spriteViewportRect = viewport.ToScreenRect(spriteRect);

            if (SDL_PointInRect(ref mousePos, ref spriteViewportRect) == SDL_bool.SDL_TRUE)
            {
                yotsuchi.color = new SDL_Color(0, 255, 0, 255);
            }
            else
            {
                yotsuchi.color = new SDL_Color(255, 255, 255, 255);
            }

            if (SDL_PointInRect(ref mousePos, ref boxViewportRect) == SDL_bool.SDL_TRUE)
            {
                boxColor = new SDL_Color(255, 255, 255, 255);
                insideBox = true;
            }
            else
            {
                boxColor = new SDL_Color(255, 0, 0, 255);
                insideBox = false;
            }
        }

        public override void OnRender()
        {
            SetColor(0, 20, 20, 255);
            Clear();
            {
                u++;
                if (u > 10)
                {
                    text.text = "FPS: " + (1 / Time.deltaTime).ToString("0.0");
                    u = 0;
                }
                text.Draw(new SDL_Rect(0, 0, text.w, text.h));

                SetColor(boxColor);
                DrawFilledBox(boxViewportRect);

                SetColor(0, 255, 0, 255);

                DrawPoint(mousePos);

                SetColor(255, 255, 255, 255);

                SetColor(255, 0, 0, 255);
                DrawOutlinedBox(spriteViewportRect);
                DrawPoint(viewport.ToScreenPoint(SDL_FPoint.zero));
                yotsuchi.Draw(spriteViewportRect, viewport.rotation);
            }
            RenderPresent();
        }
    }
}
