using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SetUpGame();
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐶","🐶",
                "🐺","🐺",
                "🐱","🐱",
                "🦁","🦁",
                "🦒", "🦒",
                "🦝","🦝",
                "🐮", "🐮",
                "🐷", "🐷"
            };

            Random random = new Random();

            foreach (TextBlock textblock in mainGrid.Children.OfType<TextBlock>())
            {
                int index = random.Next(animalEmoji.Count);
                string nextEmoji = animalEmoji[index];
                textblock.Text = nextEmoji;
                animalEmoji.RemoveAt(index);
            }
        }

        TextBlock lastTBClicked = new TextBlock();
        bool isFindingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textblock = sender as TextBlock;

            if (!isFindingMatch)
            {
                lastTBClicked = textblock;
                textblock.Visibility = Visibility.Hidden;
                isFindingMatch = true;
            }
            else if (lastTBClicked.Text == textblock.Text)
            {
                textblock.Visibility = Visibility.Hidden;
                isFindingMatch = false;
            }
            else
            {
                lastTBClicked.Visibility = Visibility.Visible;
                isFindingMatch = false;
            }
        }
    }
}
