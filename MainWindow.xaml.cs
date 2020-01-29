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

namespace Snake
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        // Fields
        int points;
        int time;
        // fuer die Richtung
        int direction;
        // fuer die Breite der Spielfeldbegrenzung
        int pillarWidth;
        // Bewegungsgeschwindigkeit
        int moveTime = 250;

        // Liste fuer die Schlange
        List<SnakeParts> snake;

        // Timer fuer die Schlangenbewegung
        DispatcherTimer timerSnake;
        DispatcherTimer playTime;

        // Klasse Apples bekannt machen
        Apples myApple;

        #endregion


        #region Methods
        // Konstruktor
        public MainWindow()
        {
            InitializeComponent();

            pillarWidth = 25;

            // Liste erzeugen
            snake = new List<SnakeParts>();

            // die Instanz erzeugen
            timerSnake = new DispatcherTimer();
            // das Intervall setzen
            timerSnake.Interval = TimeSpan.FromMilliseconds(moveTime);
            // die Methode fuer das Ereignis zuweisen
            timerSnake.Tick += new EventHandler(Timer_MoveSnake);
            // den Timer starten
            timerSnake.Start();

            playTime = new DispatcherTimer();
            playTime.Interval = TimeSpan.FromMilliseconds(1000);
            playTime.Tick += new EventHandler(Timer_Playtime);
            playTime.Start();

        }

        private void Timer_Playtime(object sender, EventArgs e)
        {
            time += 1;
            showTime.Content = time;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
            DrawPlayground();
        }


        // Methode fuer den Start des Spiels
        private void Start()
        {
            points = 0;
            time = 0;
            direction = 0;
            showPoints.Content = points;
            showTime.Content = time;

            // den Schlangenkopf erzeugen und positionieren
            SnakeHead mySnakeHead = new SnakeHead(new Point(playground.ActualWidth / 2, playground.ActualHeight / 2), Colors.Red);
            // in die Liste setzen
            snake.Add(mySnakeHead);


            myApple = new Apples(Colors.Green, 20);
            myApple.ShowApple(playground, pillarWidth);
        }


        // Methode zum Zeichenen der Begrenzung
        void DrawRectangles(Point position, double width, double height)
        {
            // einen Balken erzeugen
            Rectangle pillar = new Rectangle();
            pillar.Name = "Border";
            Canvas.SetLeft(pillar, position.X);
            Canvas.SetTop(pillar, position.Y);
            pillar.Width = width;
            pillar.Height = height;
            SolidColorBrush filling = new SolidColorBrush(Colors.Red);
            pillar.Fill = filling;
            // und hinzufuegen
            playground.Children.Add(pillar);
        }


        // zum erstellen der Begrenzung
        void DrawPlayground()
        {
            // der Balken oben
            DrawRectangles(new Point(0, 0), playground.ActualWidth, pillarWidth);
            // der Balken Rechts
            DrawRectangles(new Point(playground.ActualWidth - pillarWidth, 0), pillarWidth, playground.ActualHeight);
            // der Balken unten
            DrawRectangles(new Point(0, playground.ActualHeight - pillarWidth), playground.ActualWidth, pillarWidth);
            // der Balken links
            DrawRectangles(new Point(0, 0), pillarWidth, playground.ActualHeight);
        }


        // Methode fuer das Zeichnen und Bewegen der Schlange durch den Timer
        private void Timer_MoveSnake(object sender, EventArgs e)
        {
            // den Kopf in die angegebene Richtung bewegen
            snake[0].Move(direction);
            // und zeichnen
            snake[0].Draw(playground);
            // die Teile in einer Schleife bewegen
            for (int index = 1; index < snake.Count; index++)
            {
                snake[index].SetPosition(snake[index - 1].OldPosition);
                snake[index].Draw(playground);
            }
            // Kollision pruefen
            ProofCollision();
        }


        // zum pruefen einer kollision mit dem Schlangenkopf
        private void ProofCollision()
        {
            HitTestResult hitted = VisualTreeHelper.HitTest(playground, snake[0].Position);
            if (hitted != null)
            {
                string name = ((Shape)hitted.VisualHit).Name;
                // was haben wir getroffen?
                if (name == "Collision" || name == "Apple")
                {
                    points += 10;
                    showPoints.Content = points;
                    // einen teil hinten in der Schlange anhaengen
                    SnakeParts sPart = new SnakeParts(new Point(snake[snake.Count - 1].OldPosition.X, snake[snake.Count - 1].OldPosition.Y + snake[snake.Count - 1].Size), Colors.Black);
                    snake.Add(sPart);
                    // den alten Apfel loeschen
                    myApple.RemoveApple(playground);
                    // einen neuen Apfel erzeugen
                    myApple = new Apples(Colors.Green, 20);
                    myApple.ShowApple(playground, pillarWidth);
                }
                if (name == "Border") // || name == "Snake"
                {
                    timerSnake.Stop();
                    playTime.Stop();
                }
            }
        }

        #endregion

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // je nach Taste die Richtung setzen
            // oben
            if (e.Key == Key.Up || e.Key == Key.W)
            {
                direction = 0;
            }
            // unten
            if (e.Key == Key.Down || e.Key == Key.S)
            {
                direction = 2;
            }
            // links
            if (e.Key == Key.Left || e.Key == Key.A)
            {
                direction = 3;
            }
            // rechts
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                direction = 1;
            }
        }
    }
}
