using Cosmos.System;
using radiant.services.graphics.events;
using System;

namespace radiant.services.graphics
{
    public class Control
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;

        public int X
        {
            get => _x;
            set => _x = value;
        }

        public int Y
        {
            get => _y;
            set => _y = value;
        }

        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public event EventHandler<MouseEventArgs> MouseClick;
        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseEnter;
        public event EventHandler<MouseEventArgs> MouseExit;

        protected virtual void OnMouseClick(MouseEventArgs e) => MouseClick?.Invoke(this, e);
        protected virtual void OnMouseDown(MouseEventArgs e) => MouseDown?.Invoke(this, e);
        protected virtual void OnMouseUp(MouseEventArgs e) => MouseUp?.Invoke(this, e);
        protected virtual void OnMouseEnter(MouseEventArgs e) => MouseEnter?.Invoke(this, e);
        protected virtual void OnMouseExit(MouseEventArgs e) => MouseExit?.Invoke(this, e);

        private bool IsMouseInBounds(uint mouseX, uint mouseY) =>
            mouseX >= _x && mouseX <= (_x + _width) &&
            mouseY >= _y && mouseY <= (_y + _height);

        private bool _mouseHovering = false;

        public virtual void Update()
        {
            uint mouseX = MouseManager.X;
            uint mouseY = MouseManager.Y;

            if (IsMouseInBounds(mouseX, mouseY))
            {
                if (!_mouseHovering)
                {
                    _mouseHovering = true;
                    OnMouseEnter(new MouseEventArgs { x = mouseX, y = mouseY });
                }

                if (MouseManager.MouseState == MouseState.Left || MouseManager.LastMouseState == MouseState.None)
                {
                    OnMouseDown(new MouseEventArgs { x = mouseX, y = mouseY });
                }

                if (MouseManager.MouseState == MouseState.None || MouseManager.LastMouseState == MouseState.Left)
                {
                    OnMouseUp(new MouseEventArgs { x = mouseX, y = mouseY });
                    OnMouseClick(new MouseEventArgs { x = mouseX, y = mouseY });
                }
            }
            else if (_mouseHovering)
            {
                _mouseHovering = false;
                OnMouseExit(new MouseEventArgs { x = mouseX, y = mouseY });
            }
        }

        public virtual void Draw() { }
    }
}
