using radiant.services.graphics.controls;
using radiant.services.graphics.events;
using System.Drawing;

namespace radiant.services.graphics.windows
{
    public class TestWindow : Window
    {
        private int clickCount;
        private readonly Label countLabel;

        public TestWindow() : base("Test window", 100, 100, 300, 150)
        {
            Label welcomeLabel = new(20, 20, "Welcome to radiant!", Color.White);
            countLabel = new(20, 55, $"Clicked the button {clickCount} times", Color.White);
            Button clickBtn = new(20, 90, 200, 40, "Click me!");

            // This causes compilation error
            // temporarily commented out
            // clickBtn.MouseClick += OnClickBtnClick;

            Controls.Add(welcomeLabel);
            Controls.Add(countLabel);
            Controls.Add(clickBtn);
        }

        private void OnClickBtnClick(object sender, MouseEventArgs e)
        {
            clickCount++;
            countLabel.Text = $"Clicked the button {clickCount} times";
        }
    }
}
