using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace Snake
{
    class SnakeParts
    {
        #region Fields
        // die Position
        //protected Point _position;
        protected Point _position;
        public virtual Point Position
        {
            get { return _position; }
            set { _position = value; }
        }

        // die alte Position
        protected Point _oldPosition;
        public Point OldPosition
        {
            get { return _oldPosition; }
            set { _oldPosition = value; }
        }

        // die Farbe
        protected Color color;
        // die Form
        protected Rectangle square;

        // die Groesse
        protected int _size;
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }


        #endregion



        #region Methods
        // der Konstruktur
        public SnakeParts(Point position, Color color)
        {
            this._position.X = position.X;
            this._position.Y = position.Y;
            // alte und neue Position sind erstmal identisch
            _oldPosition.X = this._position.X;
            _oldPosition.Y = this._position.Y;
            // Farve setzen
            this.color = color;

            // die Groesse wird fest gesetzt
            _size = 20;

            // ein neues Rechteck erzeugen
            square = new Rectangle();
        }


        // eine leere Methode zum bewegen
        public virtual void Move(int direction)
        {
            // Sie wird in der abgeleit. Klasse ueberschrieben
        }


        // die neue Position setzen
        public void SetPosition(Point newPosition)
        {
            // die alte Position speichern
            OldPosition = Position;
            Position = newPosition;
        }


        // die neue Position setzen
        public void Draw(Canvas myCanvas)
        {
            // das Quadrat loeschen
            myCanvas.Children.Remove(square);
            // positionieren
            Canvas.SetLeft(square, _position.X);
            Canvas.SetTop(square, _position.Y);
            // die groesse setzen
            square.Width = _size;
            square.Height = _size;
            // Farbe und Rahmen setzen
            SolidColorBrush filling = new SolidColorBrush(color);
            square.Fill = filling;
            SolidColorBrush frame = new SolidColorBrush(Colors.White);
            square.Stroke = frame;

            // wieder hinzufuegen
            myCanvas.Children.Add(square);
        }

        

        #endregion

    }
}
