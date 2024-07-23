using System.Drawing;
using G = radiant.services.graphics.GraphicsMain;

namespace radiant.services.graphics.controls
{
    internal class Label : Control
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

        public Label(int x, int y, string text, Color color, int fontSize = 16)
        {
            X = x;
            Y = y;
            Text = text;
            FontSize = fontSize;
            Color = color;
        }

        public override void Draw()
        {
            G.RegularFont.DrawToSurface(G.surface, FontSize, X, Y, Text, Color);
        }
    }
}
