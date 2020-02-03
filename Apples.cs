using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Snake
{
    class Apples
    {
        #region Fields
        // die Farbe
        Color color;
        // die Form
        Rectangle squareCollision;
        // das Bild
        Image apple;

        // die Groesse
        int appleSize;

        private Point circleCenter;

        // Listen fuer die Kollisionsabfragen
        static List<Point> barrierPositions = new List<Point>();
        static List<Point> snakePartsPositions = new List<Point>();

        #endregion


        #region Methods
        // der Konstruktor zum etzen der Farbe und der Groesse und des Spielfeldes
        public Apples(Color color, int appleSize)
        {
            // Farbe setzen
            this.color = color;
            // die Groesse setzen
            this.appleSize = appleSize;
            // einen neuen Kreis erzeugen
            squareCollision = new Rectangle();
            squareCollision.Name = "Apple";

            apple = new Image();
            apple.Source = new BitmapImage(new Uri("pixels/apple_pxl.png", UriKind.Relative));
        }

        
        // den Apfel anzeigen
        public void ShowApple(Canvas myCanvas, int pillarWidth)
        {
            // den Zufallsgenerator initialisieren
            Random rnd = new Random();
            // das Minimum ist die Balkenbreite
            int min = pillarWidth;
            // das Maximum ermitteln
            int maxX = (int)myCanvas.ActualWidth - pillarWidth - appleSize;
            int maxY = (int)myCanvas.ActualHeight - pillarWidth - appleSize;
            // positionieren
            Canvas.SetLeft(apple, rnd.Next(min, maxX));
            Canvas.SetTop(apple, rnd.Next(min, maxY));

            // die Groesse setzen
            apple.Width = appleSize;
            apple.Height = appleSize;
            // Apfel unsichtbar machen, bis endgueltige Position gesetzt
            apple.Opacity = 0;

            // den Dummy fuer die Kollision erstellen
            squareCollision.Width = apple.Width + (appleSize - 1);
            squareCollision.Height = apple.Height + (appleSize - 1);
            // Farbe setzen
            SolidColorBrush filling = new SolidColorBrush(color);
            filling = new SolidColorBrush(Colors.Aqua);
            filling.Opacity = 0;
            squareCollision.Fill = filling;
            Canvas.SetLeft(squareCollision, Canvas.GetLeft(apple) - ((appleSize - 1) / 2));
            Canvas.SetTop(squareCollision, Canvas.GetTop(apple) - ((appleSize - 1) / 2));

            double topX = Canvas.GetTop(squareCollision);
            double topY = Canvas.GetLeft(squareCollision);

            // Kreismittelpunkt ermitteln
            circleCenter = new Point(topX + (squareCollision.Height / 2), topY + (squareCollision.Width / 2));

            // hinzufuegen
            myCanvas.Children.Add(apple);
            myCanvas.Children.Add(squareCollision);

            // Auf Kollsion im Spielfeld pruefen
            ProofCollision(myCanvas, pillarWidth);
        }


        // Methode um eine Kollision des Apfels mit den Elementen im Spielfeld zu ueberpruefen
        private void ProofCollision(Canvas myCanvas, int pillarWidth)
        {
            for (int i = 0; i < barrierPositions.Count; i++)
            {
                if (((circleCenter.X >= (barrierPositions[i].X - 35) && circleCenter.X <= (barrierPositions[i].X + 35))) && (((circleCenter.Y >= (barrierPositions[i].Y - 35) && circleCenter.Y <= (barrierPositions[i].Y + 35)))))
                {
                    RemoveApple(myCanvas);
                    ShowApple(myCanvas, pillarWidth);
                    return;
                }
            }
            for (int i = 0; i < snakePartsPositions.Count; i++)
            {
                if (((circleCenter.X >= (snakePartsPositions[i].X - 25) && circleCenter.X <= (snakePartsPositions[i].X + 25))) && (((circleCenter.Y >= (snakePartsPositions[i].Y - 25) && circleCenter.Y <= (snakePartsPositions[i].Y + 25)))))
                {
                    RemoveApple(myCanvas);
                    ShowApple(myCanvas, pillarWidth);
                    return;
                }
            }
            // Apfel sichtbar machen
            apple.Opacity = 1;
        }


        // den Apfel entfernen
        public void RemoveApple(Canvas myCanvas)
        {
            myCanvas.Children.Remove(squareCollision);
            myCanvas.Children.Remove(apple);
        }


        // Methode um die Positionen der Hindernisse in die Liste einzutragen
        public static void SetBarrierPosition(Point barrierPos)
        {
            barrierPositions.Add(barrierPos);
        }

        // zum leeren der Liste
        public static void ClearBarrierList()
        {
            barrierPositions.Clear();
        }


        // Methode um die Positionen der Schlangenteile in die Liste einzutragen
        public static void SetSnakePartsPosition(Point snakePartPos)
        {
            snakePartsPositions.Add(snakePartPos);
        }

        // zum leeren der Liste
        public static void ClearSnakePartsList()
        {
            snakePartsPositions.Clear();
        }
        #endregion
    }
}
