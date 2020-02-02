using System.Windows;
using System.Windows.Media;

namespace Snake
{
    class SnakeHead : SnakeParts
    {

        #region Methods
        // der Konstruktor
        public SnakeHead(Point position, Color color) : base(position, color)
        {

        }

        // Methode zum bewegen
        public override void Move(int direction)
        {
            // die alte Position speichern
            oldPosition = position;
            // und veraendern
            switch (direction)
            {
                // nach oben
                case 0:
                    position.Y = position.Y - size;
                    break;
                // nach rechts
                case 1:
                    position.X = position.X + size;
                    break;
                // nach unten
                case 2:
                    position.Y = position.Y + size;
                    break;
                // nach links
                case 3:
                    position.X = position.X - size;
                    break;
            }
        }


        public override Point GetPosition()
        {
            return new Point(position.X + (size / 2), position.Y + (size / 2));
        }


        #endregion
    }
}
