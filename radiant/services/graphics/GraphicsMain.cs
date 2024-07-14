using Cosmos.Core.Memory;
using Cosmos.HAL;
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

        static int frames = 0;
        static int FPS = 0;
        static int currentSecond = 0;
        static int garbageCollected = 0;

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
            RegularFont = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("FreeSans.ttf"));
            TitleFont = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("FreeSansBoldOblique.ttf"));
            canvas.Clear(Color.Blue);
            while (true)
            {
                Run();
                if (KeyboardManager.TryReadKey(out KeyEvent key) && key.Key == ConsoleKeyEx.Escape) break;
            }
            canvas.Disable();
        }

        static readonly Image cursor = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("cur.bmp"));

        static readonly Button b = new(50, 50, 50, 25, "test");

        static void Run()
        {
            canvas.Clear(Color.Black);

            b.Update();
            b.Draw();

            canvas.DrawImageAlpha(cursor, (int)MouseManager.X, (int)MouseManager.Y, true);

            RegularFont.DrawToSurface(surface, 10, 10, 10, $"--- DEBUG ---", Color.White);
            RegularFont.DrawToSurface(surface, 10, 10, 26, $"FPS: {FPS}", Color.White);
            RegularFont.DrawToSurface(surface, 10, 10, 42, $"Garbage Collected: {garbageCollected} objs", Color.White);
            RegularFont.DrawToSurface(surface, 10, 10, 58, $"Press ESC to exit", Color.White);
            TitleFont.DrawToSurface(surface, 30, 130, 130, "Hello, World!", Color.White);
            TitleFont.DrawToSurface(surface, 30, 130, 170, "Привет, мир!", Color.White);

            canvas.Display();
            frames++;

            if (RTC.Second != currentSecond)
            {
                currentSecond = RTC.Second;
                FPS = frames;
                frames = 0;
            }

            garbageCollected += Heap.Collect();

        }
    }
}
