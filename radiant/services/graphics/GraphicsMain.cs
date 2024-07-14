using Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;

namespace radiant.services.graphics
{
    internal class GraphicsMain
    {
        public const uint defaultScreenW = 640, defaultScreenH = 480;
        static uint screenw, screenh;
        static Canvas canvas;

        public static void Init(uint w = defaultScreenW, uint h = defaultScreenH)
        {
            screenw = w;
            screenh = h;
            MouseManager.ScreenWidth = w;
            MouseManager.ScreenHeight = h;
            MouseManager.X = w / 2;
            MouseManager.Y = h / 2;
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(w, h, ColorDepth.ColorDepth32));
            canvas.Clear(Color.Blue);
            while (true)
            {
                Run();
            }
        }

        static readonly Image cursor = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("cur.bmp"));

        static void Run()
        {
            canvas.Clear(Color.Black);
            canvas.DrawImageAlpha(cursor, (int)MouseManager.X, (int)MouseManager.Y, true);
            canvas.Display();
        }
    }
}
