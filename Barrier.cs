using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snake
{
    class Barrier
    {
        #region Fields
        // die Farbe
        Color color;
        // die Form
        Rectangle barrierCollision;
        Rectangle barrierHitbox;
        // die Groesse
        int barrierSize;

        private Point squareCenter;

        private Canvas tempPlayground;

        #endregion


        #region Methods

        public Barrier(Color color, int barrierSize)
        {
            // Farbe setzen
            this.color = color;
            // die Groesse setzen
            this.barrierSize = barrierSize;
            // einen neuen Kreis erzeugen
            barrierCollision = new Rectangle();
            barrierCollision.Name = "Barrier";
            barrierHitbox = new Rectangle();
            barrierHitbox.Name = "Hitbox";
        }

        // den Apfel anzeigen
        public void ShowBarrier(Canvas myCanvas, int pillarWidth)
        {
            tempPlayground = myCanvas;
            // den Zufallsgenerator initialisieren
            Random rnd = new Random(GetHashCode());
            // das Minimum ist die Balkenbreite
            int min = pillarWidth;
            // das Maximum ermitteln
            int maxX = (int)myCanvas.ActualWidth - pillarWidth - barrierSize;
            int maxY = (int)myCanvas.ActualHeight - pillarWidth - barrierSize;
            // positionieren
            Canvas.SetLeft(barrierCollision, rnd.Next(min, maxX));
            Canvas.SetTop(barrierCollision, rnd.Next(min, maxY));

            // die Groesse setzen
            barrierCollision.Width = barrierSize;
            barrierCollision.Height = barrierSize;
            // Farben setzen
            SolidColorBrush filling = new SolidColorBrush(color);
            barrierCollision.Fill = filling;

            // den Dummy fuer die Kollision erstellen
            barrierHitbox.Width = barrierCollision.Width + (barrierSize - 1);
            barrierHitbox.Height = barrierCollision.Height + (barrierSize - 1);
            // Farbe setzen
            filling = new SolidColorBrush(Colors.DarkRed);
            //filling.Opacity = 0;
            barrierHitbox.Fill = filling;
            Canvas.SetLeft(barrierHitbox, Canvas.GetLeft(barrierCollision) - ((barrierSize - 1) / 2));
            Canvas.SetTop(barrierHitbox, Canvas.GetTop(barrierCollision) - ((barrierSize - 1) / 2));


            double topX = Canvas.GetTop(barrierCollision);
            double topY = Canvas.GetLeft(barrierCollision);

            // hinzufuegen
            myCanvas.Children.Add(barrierHitbox);
            myCanvas.Children.Add(barrierCollision);

            // Kreismittelpunkt
            squareCenter = new Point(topX + (barrierCollision.Height / 2), topY + (barrierCollision.Width / 2));

            HitTestResult coll = VisualTreeHelper.HitTest(myCanvas, squareCenter);
            if (coll != null)
            {
                //MessageBox.Show(coll.VisualHit.ToString());
                RemoveBarrier(tempPlayground);
                ShowBarrier(tempPlayground, pillarWidth);
            }

        }

        // die Barriere entfernen
        public void RemoveBarrier(Canvas myCanvas)
        {
            myCanvas.Children.Remove(barrierCollision);
        }

        // liefere Position der Barrieren
        public Point GetPosition()
        {
            return squareCenter;
        }

        #endregion
    }
}