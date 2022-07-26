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
    using System.Threading;
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        int tenthOfSecElapsed;
        int matchesFound;
        string wining = "Hurray! you BEAT the high score! Wanna beat your own score?";
        string lost = "Oops, You Lost. Its okay! Try Again?";

        double highScore = 20;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthOfSecElapsed++;
            timeTextBlock.Text = (tenthOfSecElapsed/10f).ToString("0.0 sec");
            if (matchesFound == 8)
            {
                timer.Stop();
                double currentTime = Convert.ToDouble(timeTextBlock.Text.Remove(timeTextBlock.Text.IndexOf('s')));
                if (currentTime < highScore)
                {
                    Msg.Text = wining;
                    HighScore.Text = $"High Score - {currentTime} sec";
                    HighScore.FontSize = 35;
                    highScore = currentTime;
                }
                else
                {
                    Msg.Text = lost;
                };

                mainGrid.Visibility = Visibility.Hidden;
                GameFinish.Visibility = Visibility.Visible;

            }
        }

        private void SetUpGame()
        {
            HighScore.FontSize = 20;
            StartGame();
            List<string> animalEmoji = new List<string>()
            {
                "🐶","🐶",
                "🐺","🐺",
                "🐱","🐱",
                "🦁","🦁",
                "🐭", "🐭",
                "🦝","🦝",
                "🐮", "🐮",
                "🐷", "🐷"
            };

            Random random = new Random();

            foreach (TextBlock textblock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textblock.Name != "timeTextBlock")
                {
                    textblock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textblock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthOfSecElapsed = 0;
            matchesFound = 0;
        }

        private void StartGame()
        {
            GameFinish.Visibility = Visibility.Hidden;
            //GameStart.Visibility = Visibility.Visible;
            //Thread.Sleep(5000);
            GameStart.Visibility = Visibility.Hidden;
            // set timer.. after 3 make main grid visible
            mainGrid.Visibility = Visibility.Visible;
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
                matchesFound++;
                textblock.Visibility = Visibility.Hidden;
                isFindingMatch = false;
            }
            else
            {
                lastTBClicked.Visibility = Visibility.Visible;
                isFindingMatch = false;
            }
        }

        private void Button_Click_Yes(object sender, RoutedEventArgs e)
        {
            SetUpGame();
        }

        private void Button_Click_No(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
