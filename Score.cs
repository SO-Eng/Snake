using System;
using System.Collections.Generic;
using System.IO;

namespace Snake
{
    class Score
    {

        //die Felder
        int punkte;
        string fileName;
        //Anzahl der Eintraege in der Liste
        int anzahl = 10;
        //fuer die Liste
        Liste[] bestenliste;

        //die Methonden
        //der Konstruktor
        public Score()
        {
            punkte = 0;

            //eine neue Instany der Liste erstellen
            bestenliste = new Liste[anzahl];
            for (int i = 0; i < anzahl; i++)
                bestenliste[i] = new Liste();

            //den Dateinamen aus dem Pfad "zusammenbauen"
            fileName = System.AppDomain.CurrentDomain.BaseDirectory + @"\score.dat";
            if (File.Exists(fileName))
                LesePunkte();
        }

        //zum veraendern der Punkte
        public int VeraenderePunkte(int anzahl)
        {
            punkte += anzahl;
            return punkte;
        }

        //zum resetten der Punkte
        public void LoeschePunkte()
        {
            punkte = 0;
        }


        public bool NeuerEintrag(System.Windows.Window window)
        {
            string tempName = string.Empty;
            //wenn die aktuelle Punktzahl größer ist als der
            //letzte Eintrag der Liste, wird der letzte Eintrag
            //der Liste überschrieben und die Liste neu sortiert
            if (punkte > bestenliste[anzahl - 1].GetPunkte())
            {
                //den Namen beschaffen
                NameDialog neuerName = new NameDialog();

                neuerName.Owner = window;

                neuerName.ShowDialog();
                if (neuerName.DialogResult == true)
                {
                    tempName = neuerName.GetName();
                }
                neuerName.Close();

                bestenliste[anzahl - 1].SetzeEintrag(punkte, tempName);
                Array.Sort(bestenliste);
                SchreibePunkte();

                return true;
            }
            else
                return false;
        }

        //Bestenliste in das Spielfeld malen
        public void ListeAusgeben(System.Windows.Window window)
        {
            // Liste vom Typ String
            List<string> entries = new List<string>();
            for (int i = 0; i < anzahl; i++)
            {
                entries.Add(Convert.ToString(bestenliste[i].GetPunkte()));
                entries.Add(bestenliste[i].GetName());
            }
            // die Liste anzeigen
            Leaderboard lB = new Leaderboard(entries);
            lB.Owner = window;
            lB.ShowDialog();
        }


        void LesePunkte()
        {
            // temporaere Variablen
            int tempPoints;
            string tempName;

            // Instanz fuer den FileStream
            using (FileStream fStream = new FileStream(fileName, FileMode.Open))
            {
                // eine Instanz fuer den BinaryReader
                using (BinaryReader binaryFile = new BinaryReader(fStream))
                {
                    // die Eintraege lesen und zuweisen
                    for (int i = 0; i < anzahl; i++)
                    {
                        // die Punkte
                        tempPoints = binaryFile.ReadInt32();

                        // den Namen
                        tempName = binaryFile.ReadString();

                        // und zuweisen
                        bestenliste[i].SetzeEintrag(tempPoints, tempName);
                    }
                }
            }
        }

        void SchreibePunkte()
        {
            // neue Instanz fuer FileStream
            using (FileStream fStream = new FileStream(fileName, FileMode.Create))
            {
                using (BinaryWriter binaryFile = new BinaryWriter(fStream))
                {
                    for (int i = 0; i < anzahl; i++)
                    {
                        // zuerst die punkte
                        binaryFile.Write(bestenliste[i].GetPunkte());

                        // dann den Namen
                        binaryFile.Write(bestenliste[i].GetName());
                    }
                }
            }
        }
    }
}
