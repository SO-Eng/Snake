using System;

namespace Snake
{
    class Liste : IComparable
    {
        //die Felder
        int listePunkte;
        string listeName;

        //die Methonden
        //der Konstruktor
        public Liste()
        {
            //er setzt die Punkte und den Namen auf Standardwerte
            listePunkte = 0;
            listeName = "Nobody";
        }

        //die Vergleichsmethode
        public int CompareTo(object objekt)
        {
            Liste tempListe = (Liste)(objekt);
            if (this.listePunkte < tempListe.listePunkte)
                return 1;
            if (this.listePunkte > tempListe.listePunkte)
                return -1;
            else
                return 0;
        }

        //die Methode zum setzen von Eintraegen
        public void SetzeEintrag(int punkte, string name)
        {
            listePunkte = punkte;
            listeName = name;
        }

        //die Methode zum liefern der Punkte
        public int GetPunkte()
        {
            return listePunkte;
        }

        //die Methode zum liefern des Namens
        public string GetName()
        {
            return listeName;
        }

    }
}