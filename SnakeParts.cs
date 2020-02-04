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
        public Point position;


        // die alte Position
        protected Point oldPosition;

        // die Farbe
        protected Color color;
        // die Form
        internal Rectangle square;

        // die Groesse
        protected int size;


        #endregion



        #region Methods
        // der Konstruktur
        public SnakeParts(Point position, Color color)
        {
            this.position.X = position.X;
            this.position.Y = position.Y;
            // alte und neue Position sind erstmal identisch
            oldPosition.X = this.position.X;
            oldPosition.Y = this.position.Y;
            // Farve setzen
            this.color = color;

            // die Groesse wird fest gesetzt
            size = 20;

            // ein neues Rechteck erzeugen
            square = new Rectangle();
            square.Name = "Snake";
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
            oldPosition = position;
            position = newPosition;
        }


        // die neue Position setzen
        public void Draw(Canvas myCanvas)
        {
            // das Quadrat loeschen
            myCanvas.Children.Remove(square);
            // positionieren
            Canvas.SetLeft(square, position.X);
            Canvas.SetTop(square, position.Y);
            // die groesse setzen
            square.Width = size;
            square.Height = size;
            // Farbe und Rahmen setzen
            SolidColorBrush filling = new SolidColorBrush(color);
            square.Fill = filling;
            SolidColorBrush frame = new SolidColorBrush(Colors.White);
            square.Stroke = frame;

            // wieder hinzufuegen
            myCanvas.Children.Add(square);
        }


        //die alte Position liefern
        public Point GetOldPosition()
        {
            return oldPosition;
        }

        //die Größe liefern
        public int GetSize()
        {
            return size;
        }

        public virtual Point GetPosition()
        {
            return position;
        }

        #endregion

    }
}
