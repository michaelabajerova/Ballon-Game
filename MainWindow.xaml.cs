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
using System.Windows.Threading;

namespace Ballon_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        int speed = 3;
        int interval = 90;
        Random random = new Random();

        List<Rectangle> itemRemover = new List<Rectangle>();
        ImageBrush backgroundImage = new ImageBrush();
        int ballonSkins;
        int i;
        int missedBalloons;
        bool gameIsActive;
        int score;

        MediaPlayer player = new MediaPlayer();


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

            if(interval < 1)
            {
                ImageBrush balloonImage = new ImageBrush();
                ballonSkins += 1;

                if(ballonSkins > 5)
                {
                    ballonSkins = 1;
                }
                switch(ballonSkins)
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

            foreach(var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if((string)x.Tag == "balloon")
                {
                    i = random.Next(-5, 5);
                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - (i * -1));

                }
            }
        }

        private void PopBaloons(object sender, MouseButtonEventArgs e)
        {

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
            foreach(var x in MyCanvas.Children.OfType<Rectangle>())
            {
                itemRemover.Add(x);
            }
            foreach(Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }
            itemRemover.Clear();
            StartGame();
        }
    }
}
