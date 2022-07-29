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
        DispatcherTimer starttimer = new DispatcherTimer();

        double tenthOfSecElapsed;
        int SecElapsed = 4;
        int matchesFound;
        string wining = "Hurray! you BEAT the high score! Wanna beat your own score?";
        string lost = "Oops, You Lost. Its okay! Try Again?";
        string tie = "Hey Its a tie! Beat the high score if you can!";
        string newScore = "Hey! you setted a new high score. lets beat it!";
        bool inc;

        public MainWindow()
        {
            InitializeComponent();


            HighScore.Text = $"High Score - {Properties.Settings.Default.highScore} sec";

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            starttimer.Interval = TimeSpan.FromSeconds(1);
            starttimer.Tick += Start_Timer_Tick;
        }

        private void Start_Timer_Tick(object sender, EventArgs e)
        {
            SecElapsed--;
            gamestarttime.Text = SecElapsed.ToString();
            if (SecElapsed < 0)
            {
                starttimer.Stop();
                gamestarttime.Text = "3";
                GameStart.Visibility = Visibility.Hidden;
                mainGrid.Visibility = Visibility.Visible;
                SetUpGame();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (inc)
            {
                tenthOfSecElapsed++;
            }
            else
            {
                tenthOfSecElapsed--;
            }
            
            timeTextBlock.Text = (tenthOfSecElapsed/10f).ToString("0.0 sec");
            
            if (matchesFound == 8)
            {
                timer.Stop();
                double currentTime = Convert.ToDouble(timeTextBlock.Text.Remove(timeTextBlock.Text.IndexOf('s')));
                if (inc)
                {
                    Msg.Text = newScore;
                    HighScore.Text = $"High Score - {currentTime} sec";
                    Properties.Settings.Default.highScore = currentTime;
                    Properties.Settings.Default.Save();
                }
                else if (currentTime > 0.0)
                {
                    Msg.Text = wining +$"(by {currentTime} sec)";
                    double score = Properties.Settings.Default.highScore - currentTime;
                    HighScore.Text = $"High Score - {score} sec";
                    Properties.Settings.Default.highScore = score;
                    Properties.Settings.Default.Save();
                }
                else if (currentTime == 0.0) {
                    Msg.Text = tie;

                }

                HighScore.FontSize = 35;

                mainGrid.Visibility = Visibility.Hidden;
                GameFinish.Visibility = Visibility.Visible;

            }
            else if (timeTextBlock.Text == "0.0 sec")
            {
                timer.Stop();
                Msg.Text = lost;
                mainGrid.Visibility = Visibility.Hidden;
                GameFinish.Visibility = Visibility.Visible;
            }
        }

        private void SetUpGame()
        {
            HighScore.FontSize = 20;
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
            if (Double.IsPositiveInfinity(Properties.Settings.Default.highScore))
            {
                timeTextBlock.Text = "0.0 sec";
                tenthOfSecElapsed = 0;
                inc = true;
            }
            else
            {
                timeTextBlock.Text = Properties.Settings.Default.highScore + " sec";
                tenthOfSecElapsed = Properties.Settings.Default.highScore * 10;
                inc = false;
            }
            
            matchesFound = 0;
        }

        private void StartGame()
        {
            FirstScreen.Visibility = Visibility.Hidden;
            GameFinish.Visibility = Visibility.Hidden;
            GameStart.Visibility = Visibility.Visible;
            starttimer.Start();
            if (SecElapsed < 0)
                SecElapsed = 3;
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
            StartGame();
        }

        private void Button_Click_No(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Reset_High_Score(object sender, RoutedEventArgs e)
        {
            // RESET THE HIGH SCORE
            Properties.Settings.Default.highScore = Double.PositiveInfinity;
            Properties.Settings.Default.Save();


            HighScore.Text = $"High Score - {Properties.Settings.Default.highScore} sec";
        }
    }
}
