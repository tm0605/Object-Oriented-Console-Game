using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace IFN563_Assignment
{
    abstract class Rule
    {
        public static bool isValid(Position p)
        {
            if (p.isOccupied())
                return false;
            else
                return true;
        }
        public static bool isWinningMove(Position[] positions)
        {
            int p1_occupied = 0, p2_occupied = 0, n = 0;
            int[] location = new int[3];

            // horizontal
            for (int i = 0; i < positions.Length; i += 3) // index 1, 4, 7
            {
                for (int j = 0; j < 3; j++) // index, index + 1, index + 2
                {
                    if (positions[i + j].isOccupied() && positions[i + j].getToken().getPlayer().getSymbol() == 'X')
                    {
                        p1_occupied++;
                        location[n] = i + j;
                        n++;
                    }
                    else if (positions[i + j].isOccupied() && positions[i + j].getToken().getPlayer().getSymbol() == 'O')
                    {
                        p2_occupied++;
                        location[n] = i + j;
                        n++;
                    }
                    else
                    {
                        j = 3;
                    }
                }
                if (p1_occupied == 3 || p2_occupied == 3)
                {
                    for (int k = 0; k < location.Length; k++)
                    {
                        positions[location[k]].getToken().setVictory(true);
                    }
                    return true;
                }
                else
                {
                    p1_occupied = 0;
                    p2_occupied = 0;
                    location = new int[3];
                    n = 0;
                }
            }

            // vertical
            for (int i = 0; i < 3; i++) // index 1, 2, 3
            {
                for (int j = 0; j < 7; j += 3) // index, index + 3, index + 6
                {
                    if (positions[i + j].isOccupied() && positions[i + j].getToken().getPlayer().getSymbol() == 'X')
                    {
                        p1_occupied++;
                        location[n] = i + j;
                        n++;
                    }
                    else if (positions[i + j].isOccupied() && positions[i + j].getToken().getPlayer().getSymbol() == 'O')
                    {
                        p2_occupied++;
                        location[n] = i + j;
                        n++;
                    }
                    else
                    {
                        j = 7;
                    }
                }
                if (p1_occupied == 3 || p2_occupied == 3)
                {
                    for (int k = 0; k < location.Length; k++)
                    {
                        positions[location[k]].getToken().setVictory(true);
                    }
                    return true;
                }
                else
                {
                    p1_occupied = 0;
                    p2_occupied = 0;
                    location = new int[3];
                    n = 0;
                }
            }

            // diagonal
            for (int j = 0; j < positions.Length; j += 4) // index, index + 4, index + 8
            {
                if (positions[j].isOccupied() && positions[j].getToken().getPlayer().getSymbol() == 'X')
                {
                    p1_occupied++;
                    location[n] = j;
                    n++;
                }
                else if (positions[j].isOccupied() && positions[j].getToken().getPlayer().getSymbol() == 'O')
                {
                    p2_occupied++;
                    location[n] = j;
                    n++;
                }
                else
                {
                    j = positions.Length;
                }
            }
            if (p1_occupied == 3 || p2_occupied == 3)
            {
                for (int k = 0; k < location.Length; k++)
                {
                    positions[location[k]].getToken().setVictory(true);
                }
                return true;
            }
            else
            {
                p1_occupied = 0;
                p2_occupied = 0;
                location = new int[3];
                n = 0;
            }

            for (int j = 2; j < positions.Length - 2; j += 2) // index, index + 2, index + 4 but not index + 6
            {
                if (positions[j].isOccupied() && positions[j].getToken().getPlayer().getSymbol() == 'X')
                {
                    p1_occupied++;
                    location[n] = j;
                    n++;
                }
                else if (positions[j].isOccupied() && positions[j].getToken().getPlayer().getSymbol() == 'O')
                {
                    p2_occupied++;
                    location[n] = j;
                    n++;
                }
                else
                {
                    j = positions.Length;
                }
            }
            if (p1_occupied == 3 || p2_occupied == 3)
            {
                for (int k = 0; k < location.Length; k++)
                {
                    positions[location[k]].getToken().setVictory(true);
                }
                return true;
            }

            return false;
        }
        public static bool isDraw(Position[] positions)
        {
            int occupied = 0;
            for (int i = 0; i < positions.Length; i++) // count how many slots occupied
            {
                if (positions[i].isOccupied())
                    occupied++;
            }
            if (occupied == positions.Length) // if all occupied
                return true; // its a draw
            else
                return false; // game continues
        }
    }
}
