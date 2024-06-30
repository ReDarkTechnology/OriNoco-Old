using SDL2;
using System;
using System.Windows.Forms;
using static SDL2.SDL;

namespace OriNoco
{
    public class Game
    {
        public string Title
        {
            get => Window == IntPtr.Zero ? null : SDL_GetWindowTitle(Window);
            set
            {
                if (Window != IntPtr.Zero)
                    SDL_SetWindowTitle(Window, value);
            }
        }

        /// <summary>
        /// The parent control if using WinForms
        /// </summary>
        public Control ParentControl { get; private set; }

        /// <summary>
        /// The location of the SDL2 window
        /// </summary>
        public Point Location
        {
            get => Window == IntPtr.Zero ? new Point() : GetWindowLocation();
            set
            {
                if (Window != IntPtr.Zero)
                    SDL_SetWindowPosition(Window, value.x, value.y);
            }
        }

        private Point GetWindowLocation()
        {
            SDL_GetWindowPosition(Window, out int x, out int y);
            return new Point(x, y);
        }

        /// <summary>
        /// The size of the SDL2 window
        /// </summary>
        public Point Size
        {
            get => Window == IntPtr.Zero ? new Point() : GetWindowSize();
            set
            {
                if (Window != IntPtr.Zero)
                    SDL_SetWindowSize(Window, value.x, value.y);
            }
        }

        /// <summary>
        /// The size of the SDL2 renderer
        /// </summary>
        public Vector2 ViewportSize { get; private set; }
        /// <summary>
        /// SDL_Window created for this game
        /// </summary>
        public IntPtr Window { get; private set; }
        /// <summary>
        /// SDL_Renderer created for this game
        /// </summary>
        public IntPtr Renderer { get; private set; }
        /// <summary>
        /// Is the game still running
        /// </summary>
        public bool Running { get; private set; } = true;

        /// <summary>
        /// Initialize all the SDL modules but with a WinForm Control parent
        /// </summary>
        public void Initialize(Control parent)
        {
            ParentControl = parent;
            Initialize(parent.Handle);
        }

        /// <summary>
        /// Initialize all the SDL modules
        /// </summary>
        /// <param name="handle">Custom handle to parent it to</param>
        public void Initialize(IntPtr handle = default)
        {
            if (SDL_Init(SDL_INIT_VIDEO) < 0)
                Console.WriteLine($"There was an issue initializing SDL. {SDL_GetError()}");

            if (SDL_ttf.TTF_Init() < 0)
                Console.WriteLine($"There was an issue initializing SDL_ttf. {SDL_GetError()}");

            if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG | SDL_image.IMG_InitFlags.IMG_INIT_JPG) < 0)
                Console.WriteLine($"There was an issue initializing SDL_image. {SDL_GetError()}");

            if (handle == IntPtr.Zero)
            {
                Window = SDL_CreateWindow(
                    "OriNoco",
                    SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
                    640, 480,
                    SDL_WindowFlags.SDL_WINDOW_SHOWN);
            }
            else
            {
                Window = SDL_CreateWindowFrom(handle);
            }

            if (Window == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the window. {SDL_GetError()}");
            }

            Renderer = SDL_CreateRenderer(
                Window,
                -1,
                SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            if (Renderer == IntPtr.Zero)
                Console.WriteLine($"There was an issue creating the renderer. {SDL_GetError()}");

            OnInitialized();
        }

        /// <summary>
        /// A function to update everything in game!
        /// </summary>
        public void Run()
        {
            ViewportSize = GetOutputSize();
            PollEvents();
            Update();
            Render();
        }

        /// <summary>
        /// Checks to see if there are any events to be processed.
        /// </summary>
        void PollEvents()
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        Running = false;
                        break;
                }
                OnPolledEvent(e);
            }
        }

        /// <summary>
        /// Updates the game
        /// </summary>
        void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// Renders to the window.
        /// </summary>
        void Render()
        {
            OnRender();
        }

        /// <summary>
        /// Close the game
        /// </summary>
        public void Exit()
        {
            Running = false;
            SDL_DestroyRenderer(Renderer);
            SDL_DestroyWindow(Window);
            SDL_Quit();
        }

        #region Virtual Functions
        /// <summary>
        /// Called after all the SDL related code is initialized
        /// </summary>
        public virtual void OnInitialized()
        {

        }

        /// <summary>
        /// Called when there's an event on the window, 
        /// mostly covered by the Windows Forms control if you're parenting the window there
        /// </summary>
        public virtual void OnPolledEvent(SDL_Event ev)
        {

        }

        /// <summary>
        /// Called every frame after polling events
        /// </summary>
        public virtual void OnUpdate()
        {

        }

        /// <summary>
        /// Called when the game is rendering
        /// </summary>
        public virtual void OnRender()
        {
            SetColor(135, 206, 235, 255);
            Clear();
            {
                SetColor(180, 240, 255, 255);

                var rect = new Rect(0, 0, 15, 15);
                rect.position += (ViewportSize / 2) - (rect.size / 2);

                DrawFilledBox(rect);
            }
            RenderPresent();
        }
        #endregion

        #region SDL2 Port Functions
        /// <summary>
        /// Draw a cross span across the screen centered on that point
        /// </summary>
        /// <param name="point"></param>
        public void DrawPoint(Vector2 point)
        {
            SDL_RenderDrawLineF(Renderer, point.x, 0, point.x, ViewportSize.y);
            SDL_RenderDrawLineF(Renderer, 0, point.y, ViewportSize.x, point.y);
        }

        /// <summary>
        /// Draws a filled box
        /// </summary>
        public void DrawFilledBox(Rect rect) => SDL_RenderFillRectF(Renderer, ref rect);

        /// <summary>
        /// Draws an outlined box
        /// </summary>
        public void DrawOutlinedBox(Rect rect) => SDL_RenderDrawRectF(Renderer, ref rect);

        /// <summary>
        /// Sets the color of the render buffer
        /// </summary>
        public void SetColor(Color32 color) => SDL_SetRenderDrawColor(Renderer, color);

        /// <summary>
        /// Sets the color of the render buffer
        /// </summary>
        public void SetColor(Color color) => SDL_SetRenderDrawColor(Renderer, color);

        /// <summary>
        /// Sets the color of the render buffer
        /// </summary>
        public void SetColor(byte r, byte g, byte b, byte a) => SDL_SetRenderDrawColor(Renderer, r, g, b, a);

        /// <summary>
        /// Sets the color of the render buffer
        /// </summary>
        public void SetColor(byte r, byte g, byte b) => SDL_SetRenderDrawColor(Renderer, r, g, b, 255);

        /// <summary>
        /// Clears the render buffer
        /// </summary>
        public void Clear() => SDL_RenderClear(Renderer);

        /// <summary>
        /// Renders the present buffer
        /// </summary>
        public void RenderPresent() => SDL_RenderPresent(Renderer);

        /// <summary>
        /// Used when the mouse is not on the window
        /// </summary>
        private Vector2 lastMousePosition;
        /// <summary>
        /// Get the position of the mouse relative to the current window
        /// </summary>
        public virtual Vector2 GetMousePosition()
        {
            var windowFocus = SDL_GetMouseFocus();
            if (windowFocus == Window)
            {
                SDL_GetMouseState(out int mx, out int my);
                lastMousePosition = new Vector2(mx, my);
            }
            return lastMousePosition;
        }

        /// <summary>
        /// Get the resolution of the renderer
        /// </summary>
        public Vector2 GetOutputSize()
        {
            SDL_GetRendererOutputSize(Renderer, out int w, out int h);
            return new Vector2(w, h);
        }

        /// <summary>
        /// The size of the window SDL2 is rendering at
        /// </summary>
        private Point GetWindowSize()
        {
            SDL_GetWindowSize(Window, out int x, out int y);
            return new Point(x, y);
        }
        #endregion
    }
}
