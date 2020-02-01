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
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaktionslogik für Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : Window
    {
        public Leaderboard(List<string> entries)
        {
            InitializeComponent();

            int counterRow = 2;
            int counterColumn = 0;

            // die Eintraege in einer Liste verarbeiten
            foreach (string stringChain in entries)
            {
                // ein neues Label mit der Zeichenkette erzeugen
                Label myLabel = new Label();
                myLabel.Content = stringChain;
                // und im Grid positionieren
                Grid.SetRow(myLabel, counterRow);
                Grid.SetColumn(myLabel, counterColumn);
                myGrid.Children.Add(myLabel);
                // die Spalte erhoehen
                counterColumn++;

                // haben wir 2 Spalten gefuellt? Dann in die naechste Zeile
                if (counterColumn == 2)
                {
                    counterColumn = 0;
                    counterRow++;
                    myGrid.RowDefinitions.Add(new RowDefinition());
                }
            }
        }
    }
}
