using Cosmos.System;
using Cosmos.System.Graphics;
using radiant.services.graphics.controls;
using radiant.util.CosmosTTF;
using System.Drawing;

namespace radiant.services.graphics
{
    internal class GraphicsMain
    {
        public const uint defaultScreenW = 640, defaultScreenH = 480;
        static uint screenw, screenh;
        public static Canvas canvas;
        public static CGSSurface surface;

        public static TTFFont RegularFont;
        public static TTFFont TitleFont;

        public static void Init(uint w = defaultScreenW, uint h = defaultScreenH)
        {
            screenw = w;
            screenh = h;
            MouseManager.ScreenWidth = w;
            MouseManager.ScreenHeight = h;
            MouseManager.X = w / 2;
            MouseManager.Y = h / 2;
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(w, h, ColorDepth.ColorDepth32));
            surface = new CGSSurface(canvas);
            RegularFont = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("arial.ttf"));
            TitleFont = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("arialblackitalic.ttf"));
            canvas.Clear(Color.Blue);
            while (true)
            {
                Run();
            }
        }

        static readonly Image cursor = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("cur.bmp"));

        static Button b = new Button(50, 50, 50, 25, "test");

        static void Run()
        {
            canvas.Clear(Color.Black);
            b.Update();
            b.Draw();
            canvas.DrawImageAlpha(cursor, (int)MouseManager.X, (int)MouseManager.Y, true);

            TitleFont.DrawToSurface(surface, 30, 50, 130, "Hello, world!", Color.White);

            RegularFont.DrawToSurface(surface, 30, 50, 170, "Привет, мир!", Color.White);

            canvas.Display();
        }
    }
}
