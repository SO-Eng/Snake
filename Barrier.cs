using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Snake
{
    class Barrier
    {
        #region Fields
        // die Farbe
        Color color;
        // die Form
        Rectangle barrierHitbox;
        // die Groesse
        int barrierSize;

        private Point squareCenter;

        private Canvas tempPlayground;
        private Image barrierPic;
        string[] barrierPixels = { "pixels/tree_pxl.png", "pixels/brick_pxl.png", };

        #endregion


        #region Methods

        public Barrier(Color color, int barrierSize)
        {
            // Farbe setzen
            this.color = color;
            // die Groesse setzen
            this.barrierSize = barrierSize;
            // einen neuen Kreis erzeugen
            barrierHitbox = new Rectangle();
            barrierHitbox.Name = "Hitbox";

            barrierPic = new Image();
        }

        // den Apfel anzeigen
        public void ShowBarrier(Canvas myCanvas, int pillarWidth, int level)
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
            Canvas.SetLeft(barrierPic, rnd.Next(min, maxX));
            Canvas.SetTop(barrierPic, rnd.Next(min, maxY));

            // die Groesse setzen
            barrierPic.Width = barrierSize;
            barrierPic.Height = barrierSize;
            barrierPic.Source = new BitmapImage(new Uri(barrierPixels[level], UriKind.Relative));
            // Farben setzen
            SolidColorBrush filling = new SolidColorBrush(color);

            // den Dummy fuer die Kollision erstellen
            barrierHitbox.Width = barrierSize;
            barrierHitbox.Height = barrierSize;
            // Farbe setzen
            filling = new SolidColorBrush(Colors.DarkRed);
            //filling.Opacity = 0;
            barrierHitbox.Fill = filling;
            barrierHitbox.Opacity = 0.1;
            Canvas.SetTop(barrierHitbox, Canvas.GetTop(barrierPic));
            Canvas.SetLeft(barrierHitbox, Canvas.GetLeft(barrierPic));


            double topX = Canvas.GetTop(barrierPic);
            double topY = Canvas.GetLeft(barrierPic);

            // hinzufuegen
            myCanvas.Children.Add(barrierPic);
            myCanvas.Children.Add(barrierHitbox);

            // Kreismittelpunkt
            squareCenter = new Point(topX + (barrierPic.Height / 2), topY + (barrierPic.Width / 2));

            HitTestResult coll = VisualTreeHelper.HitTest(myCanvas, squareCenter);
            if (coll != null)
            {
                //MessageBox.Show(coll.VisualHit.ToString());
                RemoveBarrier(tempPlayground);
                ShowBarrier(tempPlayground, pillarWidth, level);
            }

        }

        // die Barriere entfernen
        public void RemoveBarrier(Canvas myCanvas)
        {
            myCanvas.Children.Remove(barrierPic);
            myCanvas.Children.Remove(barrierHitbox);
        }

        // liefere Position der Hindernisse
        public Point GetPosition()
        {
            return squareCenter;
        }
        #endregion
    }
}