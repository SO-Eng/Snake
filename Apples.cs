﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snake
{
    class Apples
    {
        #region Fields
        // die Farbe
        Color color;
        // die Form
        Ellipse circle;
        Rectangle squareCollision;
        // die Groesse
        int appleSize;

        private Canvas tempPlayground;
        public Point circleCenter;

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
            circle = new Ellipse();
            circle.Name = "Apple";
            squareCollision = new Rectangle();
            squareCollision.Name = "Collision";

        }

        
        // den Apfel anzeigen
        public void ShowApple(Canvas myCanvas, int pillarWidth)
        {
            tempPlayground = myCanvas;
            // den Zufallsgenerator initialisieren
            Random rnd = new Random(GetHashCode());
            // das Minimum ist die Balkenbreite
            int min = pillarWidth;
            // das Maximum ermitteln
            int maxX = (int)myCanvas.ActualWidth - pillarWidth - appleSize;
            int maxY = (int)myCanvas.ActualHeight - pillarWidth - appleSize;
            // positionieren
            Canvas.SetLeft(circle, rnd.Next(min, maxX));
            Canvas.SetTop(circle, rnd.Next(min, maxY));

            // die Groesse setzen
            circle.Width = appleSize;
            circle.Height = appleSize;
            // Farben setzen
            SolidColorBrush filling = new SolidColorBrush(color);
            circle.Fill = filling;

            // den Dummy fuer die Kollision erstellen
            squareCollision.Width = circle.Width + (appleSize - 1);
            squareCollision.Height = circle.Height + (appleSize - 1);
            // Farbe setzen
            filling = new SolidColorBrush(Colors.Aqua);
            //filling.Opacity = 0;
            squareCollision.Fill = filling;
            Canvas.SetLeft(squareCollision, Canvas.GetLeft(circle) - ((appleSize - 1) / 2));
            Canvas.SetTop(squareCollision, Canvas.GetTop(circle) - ((appleSize - 1) / 2));

            double topX = Canvas.GetTop(squareCollision);
            double topY = Canvas.GetLeft(squareCollision);

            // Kreismittelpunkt
            circleCenter = new Point(topX + (squareCollision.Height / 2), topY + (squareCollision.Width / 2));

            // hinzufuegen
            myCanvas.Children.Add(squareCollision);
            myCanvas.Children.Add(circle);

            HitTestResult coll = VisualTreeHelper.HitTest(tempPlayground, circleCenter);
            if (coll != null)
            {
                string name = ((Shape)coll.VisualHit).Name;

                if (name == "Hitbox" || name == "Barrier" || name == "Snake")
                {
                    MessageBox.Show(coll.VisualHit.ToString());
                    RemoveApple(tempPlayground);
                    ShowApple(tempPlayground, pillarWidth);
                }
            }

        }


        // den Apfel entfernen
        public void RemoveApple(Canvas myCanvas)
        {
            myCanvas.Children.Remove(squareCollision);
            myCanvas.Children.Remove(circle);
        }

        public Point GetPosition()
        {
            return circleCenter;
        }

        #endregion
    }
}
