using radiant.services.graphics.events;
using System.Drawing;
using System.Text;
using G = radiant.services.graphics.GraphicsMain;

namespace radiant.services.graphics.controls
{
    internal class Button : Control
    {
        private string _text;
        private Color _color;
        private int _fontSize;

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public int FontSize
        {
            get => _fontSize;
            set => _fontSize = value;
        }

        public Button(int x, int y, int width, int height, string text)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Text = text;
            Color = Color.White;
            FontSize = 16;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Color = Color.Black;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Color = Color.Gray;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            Color = Color.Gray;
        }

        protected override void OnMouseExit(MouseEventArgs e)
        {
            base.OnMouseExit(e);
            Color = Color.White;
        }

        public override void Draw()
        {
            G.canvas.DrawFilledRectangle(Color, Parent.X + X, Parent.Y + Parent.TitleBarHeight + Y, Width, Height);
            int textX = X + (Width - G.RegularFont.CalculateWidth(Text, FontSize)) / 2;
            Rune.TryCreate('v', out Rune r);
            int textY = Y + (Height + (int)G.RegularFont.RenderGlyphAsBitmap(r, Color, FontSize).Value.bmp.Height) / 2;
            G.RegularFont.DrawToSurface(G.surface, FontSize, textX, textY, Text, Color.Black);
        }
    }
}
