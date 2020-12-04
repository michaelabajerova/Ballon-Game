using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Ballon_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private int speed = 3;
        private int interval = 90;
        private Random random = new Random();

        private List<Rectangle> itemRemover = new List<Rectangle>();
        private ImageBrush backgroundImage = new ImageBrush();
        private int ballonSkins;
        private int i;
        private int missedBalloons;
        private bool gameIsActive;
        private int score;

        private MediaPlayer player = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            backgroundImage.ImageSource = new BitmapImage(new Uri(@"C:\Users\bajer\Documents\Portfolio\.NET\Ballon Game\Assets\background-Image.jpg"));
            MyCanvas.Background = backgroundImage;

            RestartGame();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            scoreText.Content = "Score: " + score;
            interval -= 10;

            if (interval < 1)
            {
                ImageBrush balloonImage = new ImageBrush();
                ballonSkins += 1;

                if (ballonSkins > 5)
                {
                    ballonSkins = 1;
                }
                switch (ballonSkins)
                {
                    case 1:
                        balloonImage.ImageSource = new BitmapImage(new Uri(@"C:\Users\bajer\Documents\Portfolio\.NET\Ballon Game\Assets\balloon1.png"));
                        break;

                    case 2:
                        balloonImage.ImageSource = new BitmapImage(new Uri(@"C:\Users\bajer\Documents\Portfolio\.NET\Ballon Game\Assets\balloon2.png"));
                        break;

                    case 3:
                        balloonImage.ImageSource = new BitmapImage(new Uri(@"C:\Users\bajer\Documents\Portfolio\.NET\Ballon Game\Assets\balloon3.png"));
                        break;

                    case 4:
                        balloonImage.ImageSource = new BitmapImage(new Uri(@"C:\Users\bajer\Documents\Portfolio\.NET\Ballon Game\Assets\balloon4.png"));
                        break;

                    case 5:
                        balloonImage.ImageSource = new BitmapImage(new Uri(@"C:\Users\bajer\Documents\Portfolio\.NET\Ballon Game\Assets\balloon5.png"));
                        break;
                }
                Rectangle newBallon = new Rectangle
                {
                    Tag = "balloon",
                    Height = 70,
                    Width = 60,
                    Fill = balloonImage,
                };
                Canvas.SetLeft(newBallon, random.Next(50, 400));
                Canvas.SetTop(newBallon, 600);

                MyCanvas.Children.Add(newBallon);

                interval = random.Next(90, 150);
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "balloon")
                {
                    i = random.Next(-5, 5);
                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - (i * -1));
                }
                if (Canvas.GetTop(x) < 20)
                {
                    itemRemover.Add(x);
                    missedBalloons += 1;
                }
            }
            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }

            if (missedBalloons > 10)
            {
                gameIsActive = false;
                gameTimer.Stop();
                MessageBox.Show("Game Over" + Environment.NewLine + "You missed 10 balloons" + Environment.NewLine + "Click OK to play again");

                RestartGame();
            }
            if(score > 10)
            {
                speed = 5;
            }
        }

        private void PopBaloons(object sender, MouseButtonEventArgs e)
        {
            if (gameIsActive)
            {
                if (e.OriginalSource is Rectangle)
                {
                    Rectangle activeRectangle = (Rectangle)e.OriginalSource;
                    player.Open(new Uri(@"C:\Users\bajer\Documents\Portfolio\.NET\Ballon Game\Assets\pop_sound.mp3", UriKind.RelativeOrAbsolute));
                    player.Play();

                    MyCanvas.Children.Remove(activeRectangle);
                    score += 1;
                }
            }
        }

        private void StartGame()
        {
            gameTimer.Start();
            missedBalloons = 0;
            score = 0;
            interval = 90;
            gameIsActive = true;
            speed = 3;
        }

        private void RestartGame()
        {
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                itemRemover.Add(x);
            }
            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }
            itemRemover.Clear();
            StartGame();
        }
    }
}