using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Esercizio16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char C = ' ';
            int scelta = 0;
            int contVG = 0;
            int contVR = 0;
            int contPareggio = 0;

            /*
             * Turno 1 ==> ROSSO
             * Turno 2 ==> GIALLO
             */

            do
            {
                scelta = disegnaMenu();

                switch(scelta)
                {
                    case 1:
                        int pedG = 0, pedR = 0;
                        int posI = 7, posOLD = 7;
                        int N = 0;
                        int Turno = 1;
                        bool vG = false;
                        bool vR = false;
                        bool pareggio = false;
                        int R = 19;
                        int[,] M = new int[8, 8];
                        int j = 0;
                        int posMax = 35;
                        int posMin = 7;
                        int i = 0;
                        int cont = 0;
                        int contP = 0;

                        // Creazione delle MATRICE di Gioco a 0
                        caricaMatrice(M);

                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                        // Creo la Struttura (Campo da Gioco)
                        Struttura();

                        // Gestione del Punteggio
                        Punteggio(Turno, pedG, pedR);

                        // Gestione della Pedina
                        Pedina(posI, ref posOLD, N, Turno, ref R, ref M, posMin, posMax, ref i);

                        do
                        {
                            // Controllo la "pressione" di un Tasto / Carattere
                            if (Console.KeyAvailable)
                            {
                                C = Convert.ToChar(Console.ReadKey(false).Key);
                                N = Convert.ToInt32(C);

                                switch (posI)
                                {
                                    case 7: j = 0; break;
                                    case 11: j = 1; break;
                                    case 15: j = 2; break;
                                    case 19: j = 3; break;
                                    case 23: j = 4; break;
                                    case 27: j = 5; break;
                                    case 31: j = 6; break;
                                    case 35: j = 7; break;
                                }

                                switch (N)
                                {
                                    case 37:
                                        // Freccia SX
                                        if (posI > posMin)
                                            posI -= 4;
                                        break;
                                    case 39:
                                        // Freccia DX
                                        if (posI < posMax)
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

                                        // Controllo se la colonna è piena
                                        while ((M[0, j] == 1 || M[0, j] == 2) && posI == posMax && !pareggio)
                                        {
                                            posOLD = posI;
                                            posI -= 4;
                                            posMax -= 4;
                                            Console.SetCursorPosition(posOLD, 1);
                                            Console.Write("    ");
                                            j--;
                                        }

                                        while ((M[0, j] == 1 || M[0, j] == 2) && posI > posMin && posI < posMax && !pareggio)
                                        {
                                            posOLD = posI;
                                            Console.SetCursorPosition(posOLD, 1);
                                            Console.Write("     ");
                                            posI += 4;
                                            j++;
                                        }

                                        while ((M[0, j] == 1 || M[0, j] == 2) && posI == posMin && !pareggio)
                                        {
                                            posOLD = posI;
                                            Console.SetCursorPosition(posOLD, 1);
                                            Console.Write("     ");
                                            posI += 4;
                                            posMin += 4;
                                            j++;
                                        }

                                        // Gestione del Punteggio
                                        Punteggio(Turno, pedG, pedR);

                                        break;

                                }

                                // Gestione della Pedina
                                Pedina(posI, ref posOLD, N, Turno, ref R, ref M, posMin, posMax, ref i);
                                contP++;

                                // Controllo Nuova Pedina - Forza 4
                                ControlloColonna(M, Turno, ref vG, ref vR, j, i, ref cont);
                                if (cont != 4)
                                    ControlloDP(M, Turno, ref vG, ref vR, j, i, ref cont);
                                if (cont != 4)
                                    ControlloRiga(M, Turno, ref vG, ref vR, i, ref cont);
                                if (cont != 4)
                                    ControlloDS(M, Turno, ref vG, ref vR, j, i, ref cont);

                                if (cont == 4)
                                {
                                    if (Turno == 2)
                                        vR = true;
                                    else
                                        vG = true;
                                }
                                else if (contP == 64)
                                    pareggio = true;
                            }

                        }
                        while ((C != 81) && !vG && !vR && !pareggio); // a Scelta !!!

                        Console.SetCursorPosition(0, 25);
                        if (vG)
                        {
                            Console.WriteLine("Vittoria del giallo");
                            contVG++;
                        }
                        else if (vR)
                        {
                            Console.WriteLine("Vittoria del rosso");
                            contVR++;
                        }
                        else if (pareggio)
                        {
                            Console.WriteLine("Pareggio");
                            contPareggio++;
                        }
                        attesa();
                        break;

                    case 2:
                        if (contVG > 0 || contVR > 0 || contPareggio > 0)
                            visualizzaRisultati(contVG, contVR, contPareggio);
                        else
                            Console.WriteLine("Gioca almeno una partita per vedere i risultati");
                        break;
                }
            }
            while (scelta != 0);
            

        } // FINE MAIN

        public static void visualizzaRisultati(int VG, int VR, int Par)
        {
            Console.Clear();
            Console.WriteLine("----RISULTATI----");
            Console.WriteLine($"Vittorie Rosso: {VR}");
            Console.WriteLine($"Vittorie Giallo: {VG}");
            Console.WriteLine($"Pareggi :{Par}");

            attesa();
        }

        public static int disegnaMenu()
        {
            int sc = 0;

            Console.Clear();
            Console.WriteLine("----FORZA 4----");
            Console.WriteLine("1. Gioca");
            Console.WriteLine("2. Visualizza Risultati");
            Console.WriteLine("0. ESCI");

            Console.WriteLine();
            Console.WriteLine("Inserisci la tua scelta == >");
            sc = Convert.ToInt32(Console.ReadLine());

            return sc;
        }

        public static void ControlloDS(int[,] M, int T, ref bool vG, ref bool vR, int j, int i, ref int cont)
        {
            cont = 0;
            int x = 0;

            if (T == 2)
                x = 1;
            else
                x = 2;

            while (i > 0 && j < 7)
            {
                j++;
                i--;
            }

            while (i < 7 && j > 0 && cont < 4)
            {
                if (x == M[i, j])
                    cont++;
                else
                {
                    cont = 1;
                }

                i++;
                j--;
            }
        }

        public static void ControlloRiga(int[,] M, int T, ref bool vG, ref bool vR, int i, ref int cont)
        {
            cont = 0;
            int j = 0;
            int x = M[i, j];
            vG = false;
            vR = false;

            while (j < 8 && cont < 4) 
            {
                if (M[i, j] == x && M[i,j] != 0)
                    cont++;
                else
                {
                    cont = 1;
                    x = M[i, j];
                }
                j++;
            }
        }

        public static void ControlloDP(int[,] M, int T, ref bool vG, ref bool vR, int j, int i, ref int cont)
        {
            cont = 0;
            int x = 0;

            if (T == 2)
                x = 1;
            else
                x = 2;

            while (i > 0 && j > 0)
            {
                j--;
                i--;
            }

            while (i < 8 && j < 8)
            {
                if (x == M[i, j])
                    cont++;
                else
                {
                    cont = 0;
                }

                i++;
                j++;
            }
        }

        static void ControlloColonna(int[,] M, int T, ref bool vG, ref bool vR, int j, int i, ref int cont)
        {
            cont = 0;
            i = 0;
            int x = M[i, j];
            vG = false;
            vR = false;

            while (i < 8 && cont < 4)
            {
                if (M[i, j] == x && M[i, j] != 0)
                    cont++;
                else
                {
                    cont = 1;
                    x = M[i, j];
                }
                i++;
            }
        }

        static void attesa()
        {
            Console.WriteLine();
            Console.WriteLine("Premi un tasto per uscire");
            Console.ReadKey();
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

        static void Pedina(int newPos, ref int oldPos, int nChar, int T, ref int R, ref int[,] M, int posMin, int posMax,ref int i)
        {
            /*
             * 37 ==> freccia SX;
             * 38 ==> freccia UP;
             * 39 ==> freccia DX;
             * 40 ==> freccia DOWN
             */

            i = 7;
            int j = 0;
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

            if (nChar == 37 && oldPos > posMin)
                oldPos -= 4;
            else if (nChar == 39 && oldPos < posMax)
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
                        oldPos = newPos;
                        if (newPos == 35)
                        {
                            newPos -= 4;
                            j--;
                        }
                        else
                        {
                            newPos += 4;
                            j++;
                        }
                        R = 19;
                        i = 7;
                        break;
                    }
                }

                if (T == 1)
                    Console.BackgroundColor = ConsoleColor.Yellow;
                else
                    Console.BackgroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(newPos, R);
                Console.Write("#");
                if (T == 1)
                    M[i, j] = 2;
                else
                    M[i, j] = 1;
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

        }
    }//FINE CLASSI
}