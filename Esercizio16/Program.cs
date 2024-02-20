using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Esercizio16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int pedG = 0, pedR = 0;
            char C = ' ';
            int posI = 7, posOLD = 7;
            int N = 0;
            int Turno = 1;
            bool vinto = false;
            bool perso = false;
            int R = 19;
            int Rm = 7;
            int Cm = 7;


            /*
             * Turno 1 ==> ROSSO
             * Turno 2 ==> GIALLO
             */

            // Creazione delle MATRICE di Gioco a 0
            int[,] M = new int[8,8];
            caricaMatrice(M);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            // Creo la Struttura (Campo da Gioco)
            Struttura();

            // Gestione del Punteggio
            Punteggio(Turno, pedG, pedR);

            // Gestione della Pedina
            Pedina(posI, ref posOLD, N, Turno, ref R, ref M);

            do
            {

                // Controllo la "pressione" di un Tasto / Carattere
                if (Console.KeyAvailable)
                {
                    C = Convert.ToChar(Console.ReadKey(false).Key);
                    N = Convert.ToInt32(C);

                    switch (N)
                    {
                        case 37:
                            // Freccia SX
                            if (posI > 7)
                                posI -= 4;
                            break;
                        case 39:
                            // Freccia DX
                            if (posI < 35)
                                posI += 4;
                            break;
                        case 40:
                            // Freccia DOWN
                            if (Turno == 1)
                            {
                                pedR++;
                                Turno++;
                            }
                            else
                            {
                                pedG++;
                                Turno--;
                            }

                            // Gestione del Punteggio
                            Punteggio(Turno, pedG, pedR);

                            // Controllo Nuova Pedina - Forza 4
                            // ...
                            break;

                    }

                    // Gestione della Pedina
                    Pedina(posI, ref posOLD, N, Turno, ref R, ref M);
                }

            }
            while ((C != 81) && !vinto && !perso); // a Scelta !!!

        }

        static void caricaMatrice(int[,] M)
        {
            for (int i = 1; i < 8; i++)
            {
                for (int j = 2; j < 8; j++)
                {
                    M[i, j] = 0;
                }
                    
            }
        }

        static void Struttura()
        {
            int ColI = 5;
            int ColY = ColI;
            Console.Clear();

            for(int C=ColI; C<65; C++)
            {
                Console.SetCursorPosition(C, 2);
                Console.Write("-");
            }

            for(int R=4; R<21; R++)
            {
                for(int C=ColI; C<38; C++)
                {
                    Console.SetCursorPosition(C,R);
                    if (C == ColY)
                    {
                        Console.Write("|");
                        ColY += 4;
                    }
                    else
                    {
                        if (R % 2 == 0)
                            Console.Write("=");
                        else
                            Console.Write(" ");
                    }

                }

                ColY = ColI;
            }

            Console.SetCursorPosition(0,0);

        }

        static void Punteggio(int T, int PG, int PR)
        {

            Console.SetCursorPosition(45,4);
            Console.Write("Turno");
            Console.SetCursorPosition(52, 4);

            Console.ForegroundColor = ConsoleColor.Black;

            if (T == 1)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("  ROSSO  ");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.Write(" GIALLO  ");
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(45, 6);
            Console.Write($"Pedine ROSSE: {PR}");

            Console.SetCursorPosition(45, 7);
            Console.Write($"Pedine GIALLE: {PG}");

        }

        static void Pedina(int newPos, ref int oldPos, int nChar, int T, ref int R, ref int[,] M)
        {
            /*
             * 37 ==> freccia SX;
             * 38 ==> freccia UP;
             * 39 ==> freccia DX;
             * 40 ==> freccia DOWN
             */
            
            int i = 7;
            int j = 0;
            bool trovato = false;
            R = 19;

            // Pulire "Vecchia" Posizione
            Console.SetCursorPosition(oldPos, 1);
            Console.Write("     ");

            Console.SetCursorPosition(newPos, 1);
            if (T == 1)
                Console.BackgroundColor = ConsoleColor.Red;
            else
                Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("#");  // a Scelta !!!

            if (nChar == 37 && oldPos > 7)
                oldPos -= 4;
            else if (nChar == 39 && oldPos < 35)
                oldPos +=4;
            else if(nChar == 40)
            {
                switch (newPos)
                {
                    case 7: j = 0;break;
                    case 11: j = 1; break;
                    case 15: j = 2; break;
                    case 19: j = 3; break;
                    case 23: j = 4; break;
                    case 27: j = 5; break;
                    case 31: j = 6; break;
                    case 35: j = 7; break;
                }

                while (M[i,j]!=0)
                {
                    R -= 2;
                    i--;
                    if (R < 4)
                    {
                        if (newPos == 35)
                            newPos -= 4;
                        else
                            newPos += 4;
                        R = 19;
                        i = 7;
                        break;
                    }
                }

                Console.SetCursorPosition(newPos, R);
                Console.Write("#");
                if (T == 1)
                    M[i, j] = 1;
                else
                    M[i, j] = 2;
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}
