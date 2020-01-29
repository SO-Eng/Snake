using System;
using System.Collections.Generic;
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
            //base.Move(direction);
            // die alte Position speichern
            OldPosition = Position;
            // und veraendern
            switch (direction)
            {
                // nach oben
                case 0:
                    _position.Y = Position.Y - Size;
                    break;
                // nach rechts
                case 1:
                    _position.X = Position.X + Size;
                    break;
                // nach unten
                case 2:
                    _position.Y = Position.Y + Size;
                    break;
                // nach links
                case 3:
                    _position.X = Position.X - Size;
                    break;
            }
        }


        public override Point Position { get => base.Position; set => base.Position = value; }


        #endregion
    }
}
