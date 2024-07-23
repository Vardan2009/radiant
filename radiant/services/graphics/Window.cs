using Cosmos.System;
using System.Collections.Generic;
using System.Drawing;
using G = radiant.services.graphics.GraphicsMain;

namespace radiant.services.graphics
{
    public class Window
    {
        public string Title { get; protected set; }
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int W { get; protected set; }
        public int H { get; protected set; }
        public List<Control> Controls { get; protected set; } = new();
        public Color BackgroundColor { get; protected set; } = Color.FromArgb(255, 35, 34, 36);
        public Color TitleBarColor { get; protected set; } = Color.FromArgb(255, 88, 44, 102);
        public int TitleBarHeight { get; protected set; } = 25;



        public Window(string title, int x, int y, int w, int h)
        {
            Title = title;
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        protected bool IsMouseInTitleBar() =>
            MouseManager.X >= X && MouseManager.X <= (X + W) &&
            MouseManager.Y >= Y && MouseManager.Y <= (Y + TitleBarHeight);

        public void InitControls()
        {
            foreach (Control control in Controls)
            {
                control.Parent = this;
            }
        }

        public void Update()
        {
            if (IsMouseInTitleBar() && MouseManager.MouseState == MouseState.Left)
            {
                X += MouseManager.DeltaX;
                Y += MouseManager.DeltaY;
            }

            G.canvas.DrawFilledRectangle(BackgroundColor, X, Y, W, H, true);
            G.canvas.DrawFilledRectangle(TitleBarColor, X, Y, W, TitleBarHeight, true);
            G.TitleFont.DrawToSurface(G.surface, 20, X + 5, Y + TitleBarHeight / 2, Title, Color.White);

            foreach (Control control in Controls)
            {
                control.Update();
            }
        }
    }
}
