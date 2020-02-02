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
        int addPoints;
        int time;
        // fuer die Richtung
        int direction;
        // fuer die Breite der Spielfeldbegrenzung
        int pillarWidth;
        // Geschwindigkeit der Schlange
        int speedSnake;
        int oldSpeedSnake;

        // Doppelte Geschwindigkeitsabfrage
        bool speedUp = false;

        // Liste fuer die Schlange
        List<SnakeParts> snake;

        // Timer fuer die Schlangenbewegung
        DispatcherTimer timerSnake;
        DispatcherTimer timerGametime;

        // Klasse Apples bekannt machen
        Apples myApple;
        Barrier myBarrier;

        // Spiel pausert?
        bool gameBreak;
        bool gameStarted;

        // Klasse Score instanzieren
        Score gamePoints;

        // Commands
        public static RoutedCommand BreakKey { get; } = new RoutedCommand();
        public static RoutedCommand NewGameKey { get; } = new RoutedCommand();

        public Color BorderColor;
        private int _counterLevel;
        // 10 = leicht; 5 = mittel; 2 = schwer
        private int difficulty;

        #endregion


        #region Methods
        // Konstruktor
        public MainWindow()
        {
            // Break fuer Splashscreen
            System.Threading.Thread.Sleep(2000);

            InitializeComponent();

            pillarWidth = 20;
            speedSnake = 500;

            // Liste erzeugen
            snake = new List<SnakeParts>();

            // Timer fuer die Schlangenbewegung
            timerSnake = new DispatcherTimer();
            timerSnake.Interval = TimeSpan.FromMilliseconds(speedSnake);
            timerSnake.Tick += new EventHandler(Timer_MoveSnake);

            // Timer fuer die Zeitanzeige
            timerGametime = new DispatcherTimer();
            timerGametime.Interval = TimeSpan.FromMilliseconds(1000);
            timerGametime.Tick += new EventHandler(Timer_Playtime);

            // Spiel zu beginn anhalten
            gameBreak = true;
            gameStarted = false;

            // Einstellungen aktivieren
            MenuEasy.IsEnabled = true;
            MenuAvarage.IsEnabled = true;
            MenuHeavy.IsEnabled = true;

            // Mittel
            addPoints = 10;
            difficulty = 6;

            gamePoints = new Score();
            
        }


        private void Timer_Playtime(object sender, EventArgs e)
        {
            time += 1;
            showTime.Content = time;
            // SpeedUp subtrahieren, wenn aktiv
            if (speedUp)
            {
                ProgressBarSpeed.Value -= 1;
            }
            // Wenn 0 dann Schlange verlangsamen
            if (ProgressBarSpeed.Value == 0 && speedUp)
            {
                speedSnake = oldSpeedSnake;
                timerSnake.Interval = TimeSpan.FromMilliseconds(speedSnake);
                speedUp = false;
            }
        }

        private void GameClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DrawPlayground();
            NextLevel(0);
        }


        // Methode fuer den Start des Spiels
        private void Start()
        {
            // Geschwindigkeit setzen
            speedSnake = 500;
            timerSnake.Interval = TimeSpan.FromMilliseconds(speedSnake);
            gameStarted = true;
            gamePoints.LoeschePunkte();
            ProgressBarSpeed.Value = 0;
            ProgressBarLife.Value = 3;
            // Liste leeren
            snake.Clear();
            // Spielfeld leeren
            playground.Children.Clear();

            points = 0;
            time = 0;
            direction = 0;
            showPoints.Content = points;
            showTime.Content = time;

            NextLevel(0);
            //DrawPlayground();

            myApple = new Apples(Colors.Green, 25);
            myApple.ShowApple(playground, pillarWidth);

            // Einstellungen deaktivieren
            MenuEasy.IsEnabled = false;
            MenuAvarage.IsEnabled = false;
            MenuHeavy.IsEnabled = false;

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
            SolidColorBrush filling = new SolidColorBrush(BorderColor);
            pillar.Fill = filling;
            // und hinzufuegen
            playground.Children.Add(pillar);
        }


        // zum erstellen der Begrenzung
        internal void DrawPlayground()
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
                snake[index].SetPosition(snake[index - 1].GetOldPosition());
                snake[index].Draw(playground);
            }
            // Kollision pruefen
            ProofCollision();
        }


        // zum pruefen einer kollision mit dem Schlangenkopf
        private void ProofCollision()
        {
            //HitTestResult collAplle = VisualTreeHelper.HitTest(playground, myApple.GetPosition());
            HitTestResult hitted = VisualTreeHelper.HitTest(playground, snake[0].GetPosition());
            if (hitted != null)
            {
                string name = ((Shape)hitted.VisualHit).Name;
                // was haben wir getroffen?
                if (name == "Border" || name == "Snake")
                {
                    EndGame();
                }
                if (name == "Collision" || name == "Apple")
                {
                    points = gamePoints.VeraenderePunkte(addPoints);
                    showPoints.Content = points;
                    // Geschwindigkeit der Schlange erhoehen, wenn nicht das Maximum schon erreicht ist
                    //if (points % 50 == 0 && speedSnake > 100)
                    //{
                    //    speedSnake -= 100;
                    //    timerSnake.Interval = TimeSpan.FromMilliseconds(speedSnake);
                    //    if (speedUp)
                    //    {
                    //        oldSpeedSnake -= 100;
                    //    }
                    //}
                    if (points % 100 == 0)
                    {
                        ProgressBarSpeed.Value += 1;
                    }
                    // einen teil hinten in der Schlange anhaengen
                    SnakeParts sPart = new SnakeParts(new Point(snake[snake.Count - 1].GetOldPosition().X, snake[snake.Count - 1].GetOldPosition().Y + snake[snake.Count - 1].GetSize()), Colors.Black);
                    snake.Add(sPart);
                    // den alten Apfel loeschen
                    myApple.RemoveApple(playground);
                    // einen neuen Apfel erzeugen
                    myApple = new Apples(Colors.Green, 25);
                    myApple.ShowApple(playground, pillarWidth);
                    if (points >= 1000)
                    {
                        GameBreak();
                        MessageBox.Show("Glückwunsch");
                        NextLevel(_counterLevel);
                    }
                }
            }

            //if (collAplle != null)
            //{
            //    myApple.RemoveApple(playground);
            //    myApple = new Apples(Colors.Green, 25);
            //    myApple.ShowApple(playground, pillarWidth);
            //}
        }

        // Bewegung der Schlange
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Spiel pausiert? Tasten nicht annehmen!
            if (gameBreak)
            {
                return;
            }
            // je nach Taste die Richtung setzen
            // oben
            if ((e.Key == Key.Up || e.Key == Key.W) && direction != 2)
            {
                direction = 0;
            }
            // unten
            if ((e.Key == Key.Down || e.Key == Key.S) && direction != 0)
            {
                direction = 2;
            }
            // links
            if ((e.Key == Key.Left || e.Key == Key.A) && direction != 1)
            {
                direction = 3;
            }
            // rechts
            if ((e.Key == Key.Right || e.Key == Key.D) && direction != 3)
            {
                direction = 1;
            }
            // SpeedUp Funktion
            if (e.Key == Key.Space || e.Key == Key.LeftShift)
            {
                SpeedUp();
            }
        }


        // Methode um die Schlange auf das doppelte ihrer Geschwindigkeit zu bringen
        private void SpeedUp()
         {
            if (ProgressBarSpeed.Value > 0 && !speedUp)
            {
                    oldSpeedSnake = speedSnake;
                    speedSnake /= 2;
                    timerSnake.Interval = TimeSpan.FromMilliseconds(speedSnake);
                    speedUp = true;
            }
            else if (!speedUp)
            {
                return;
            }
            else
            {
                speedSnake = oldSpeedSnake;
                timerSnake.Interval = TimeSpan.FromMilliseconds(speedSnake);
                speedUp = false;
            }
        }


        // erzeugt einen Dialog zum Neustart und liefert das Ergebnis zurueck
        bool NewGame()
        {
            bool result = false;
            // einen Dialog mit Ja/Nein erzeugen
            MessageBoxResult question = MessageBox.Show("Wollen Sie ein neues Spiel starten?", "Neues Spiel", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // wurde auf Ja geclickt?
            if (question == MessageBoxResult.Yes)
            {
                // dann rufen wir die Methode zum Starten auf
                Start();
                result = true;
            }
            return result;
        }


        private void GameNewGame_Click(object sender, RoutedEventArgs e)
        {
            if (!gameBreak)
            {
                GameBreak();
                // Dialog amzeigen
                NewGame();
                // und weiter spielen
                GameBreak();
            }
            // wenn kein Spiel laeuft, starten wir ein neues Spiel, wenn Ja geclickt wurde
            else
            {
                if (NewGame())
                {
                    GameBreak();
                }
            }
        }

        // Spiel pausieren
        void GameBreak()
        {
            // lauft das Spiel?
            if (!gameBreak)
            {
                // alle Timer anhalten
                timerGametime.Stop();
                timerSnake.Stop();

                // die Markierung im Menue einschalten
                GameBreakSwitch.IsChecked = true;

                // Einstellungen aktivieren
                MenuEasy.IsEnabled = true;
                MenuAvarage.IsEnabled = true;
                MenuHeavy.IsEnabled = true;

                // den Text in der Titelleiste anpassen
                Title = "Snake - Das Spiel ist pausiert!";
                gameBreak = true;
            }
            else
            {
                // alle Timer weider starten
                timerGametime.Start();
                timerSnake.Start();

                // die Markierung
                GameBreakSwitch.IsChecked = false;

                Title = "Snake";
                gameBreak = false;
            }
        }


        // Spiel beenden
        void EndGame()
        {
            // das Spiel anhalten
            GameBreak();
            // eine Meldung anzeigen
            MessageBox.Show("Schade", "Game Over!", MessageBoxButton.OK, MessageBoxImage.Information);

            // reicht es fuer einen neuen eintrg in der Betsenliste?
            if (gamePoints.NeuerEintrag(this))
            {
            }
            // Abfrage ob ein neues Spiel gestartet werden soll
            if (NewGame())
            {
                Start();
                // das Spiel "fortsetzen"
                GameBreak();
            }
            else
            {
                Close();
            }
        }

        // Schwieriegkeitsgrad Einstellungen
        private void MenuEasy_Click(object sender, RoutedEventArgs e)
        {
            // Markierung bei den anderen Eintraegen abschalten
            MenuAvarage.IsChecked = false;
            MenuHeavy.IsChecked = false;
            SetSettings(1000, 800, 1);
            ProgressBarSpeed.Height = 300;
            difficulty = 11;
        }

        private void MenuAvarage_Click(object sender, RoutedEventArgs e)
        {
            // Markierung bei den anderen Eintraegen abschalten
            MenuEasy.IsChecked = false;
            MenuHeavy.IsChecked = false;
            SetSettings(800, 600, 10);
            ProgressBarSpeed.Height = 300;
            difficulty = 6;
        }

        private void MenuHeavy_Click(object sender, RoutedEventArgs e)
        {
            // Markierung bei den anderen Eintraegen abschalten
            MenuAvarage.IsChecked = false;
            MenuEasy.IsChecked = false;
            SetSettings(400, 300, 25);
            ProgressBarSpeed.Height = 100;
            difficulty = 2;
        }

        void SetSettings(int width, int height, int pointsNew)
        {
            // die Groesse des Fensters setzen
            Width = width;
            Height = height;
            addPoints = pointsNew;
            // Fenster neu positionieren
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
            // die Elemente im Spielfeld loeschen
            playground.Children.Clear();
            // das Spielfeld neu erstellen
            DrawPlayground();
        }


        // Bestenliste anzeigen
        private void GameLeaderBoard_Click(object sender, RoutedEventArgs e)
        {
            bool forward = false;

            if (!gameBreak)
            {
                GameBreak();
                forward = true;
            }
            // Betsenliste anzeigen
            gamePoints.ListeAusgeben(this);
            if (forward)
            {
                GameBreak();
            }
        }


        // KeyBindings
        private void Break_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            // gibt es das Spielfeld und laeuft das Spiel?
            if (playground != null && gameStarted)
            {
                e.CanExecute = true;
            }
        }

        private void Break_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // pauseiren?
            GameBreak();
        }


        // KeyBindings
        private void NewGame_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            // Kann jedereit aufgerufen werden
            if (playground != null)
            {
                e.CanExecute = true;
            }
        }

        private void NewGame_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // pauseiren?
            if (!gameBreak)
            {
                GameBreak();
                // Dialog amzeigen
                NewGame();
                // und weiter spielen
                GameBreak();
            }
            // wenn kein Spiel laeuft, starten wir ein neues Spiel, wenn Ja geclickt wurde
            else
            {
                if (NewGame())
                {
                    GameBreak();
                }
            }
        }

        // Schlange erzeugen (nur Kopf)
        private void AddSnake()
        {
            // den Schlangenkopf erzeugen und positionieren
            SnakeHead mySnakeHead = new SnakeHead(new Point(playground.ActualWidth / 2, playground.ActualHeight / 2), Colors.Red);
            // in die Liste setzen
            snake.Add(mySnakeHead);

        }


        // Levelmanager-Methoden
        public void NextLevel(int level)
        {
            _counterLevel = level;
            _counterLevel++;
            switch (_counterLevel)
            {
                case 1:
                    FirstLevel();
                    break;
                case 2:
                    SecondLevel();
                    break;
            }
        }


        private void FirstLevel()
        {
            myGrid.Background = new SolidColorBrush(Color.FromRgb(97, 213, 122));
            BorderColor = Colors.SaddleBrown;
            DrawPlayground();
            AddSnake();
            for (int i = 0; i < difficulty; i++)
            {
                myBarrier = new Barrier(Colors.BlueViolet, 35);
                myBarrier.ShowBarrier(playground, pillarWidth);
            }
        }

        private void SecondLevel()
        {
            System.Threading.Thread.Sleep(1000);
            snake.Clear();
            playground.Children.Clear();
            AddSnake();
            myGrid.Background = new SolidColorBrush(Color.FromRgb(113, 136, 220));
            BorderColor = Color.FromRgb(106, 106, 106);
            DrawPlayground();
            GameBreak(); 
            for (int i = 0; i < difficulty; i++)
            {
                myBarrier = new Barrier(Colors.BlueViolet, 35);
                myBarrier.ShowBarrier(playground, pillarWidth);
            }
            myApple.ShowApple(playground,pillarWidth);
        }

        #endregion

    }
}
