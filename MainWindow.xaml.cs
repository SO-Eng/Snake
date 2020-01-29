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
        int moveTime = 1000;

        // Liste fuer die Schlange
        List<SnakeParts> snake;

        // Timer fuer die Schlangenbewegung
        DispatcherTimer timerSnake;

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

            // zum Test ein paar Teile erzeugen
            for (int index = 0; index < 10; index++)
            {
                SnakeParts sPart = new SnakeParts(new Point(snake[index].OldPosition.X, snake[index].OldPosition.Y + snake[index].Size), Colors.Black);
                snake.Add(sPart);
            }
        }


        // Methode zum Zeichenen der Begrenzung
        void DrawRectangles(Point position, double width, double height)
        {
            // einen Balken erzeugen
            Rectangle pillar = new Rectangle();
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
        }

        #endregion
    }
}
