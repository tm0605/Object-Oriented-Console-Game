using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace IFN563_Assignment
{
    internal class Board
    {
        private int Row { get; set; }
        private int Col { get; set; }
        private Position[] Position { get; set; }
        public const int BOXSIZE = 3;
        const string INTERSECTION = "+";
        const string V_LINE = "|";
        const string H_LINE = "-";
        const string PLACEHOLDER = " ";
        const ConsoleColor AVAILABLE = ConsoleColor.Green, WIN = ConsoleColor.Yellow;
        public Board(int row, int col)
        {
            Row = row;
            Col = col;
            Position = new Position[col * row];
            for (int i = 0; i < Position.Length; i++)
            {
                Position[i] = new Position(i + 1);
            }
        }
        public Position[] getPositions()
        {
            return Position;
        }
        public Position getPosition(int index)
        {
            return Position[index - 1];
        }
        public void render()
        {
            Clear();
            int r = Row * 2 + 1;
            int c = Col * 2 + 1;
            int x, y = 0;
            int width = BOXSIZE * 2 + 1, height = BOXSIZE;


            for (int ci = 0; ci < c; ci++) // Each column line
            {
                x = 0;

                if (ci % 2 == 0)
                {
                    // Draw Line

                    for (int ri = 0; ri < r; ri++)
                    {
                        if (ri % 2 == 0)
                        {
                            // Intersection
                            Write(INTERSECTION);
                            x++;
                        }
                        else
                        {
                            // BoxLine
                            for (int i = 0; i < width; i++)
                            {
                                Write(H_LINE);
                                x++;
                            }
                        }
                    }
                    y++;
                }
                else
                {
                    // Draw row

                    for (int ri = 0; ri < r; ri++)
                    {
                        SetCursorPosition(x, y);
                        if (ri % 2 == 0)
                        {
                            // Vertical Line

                            for (int i = 0; i < height; i++)
                            {
                                SetCursorPosition(x, y + i);
                                Write(V_LINE);
                            }
                            x++;
                        }
                        else
                        {
                            // Box

                            int index = ci / 2 * Row + ri / 2 + ri % 2;
                            for (int i = 0; i < height; i++)
                            {
                                SetCursorPosition(x, y + i);
                                if (!Position[index - 1].isOccupied()) // Highlight vacant position
                                {
                                    ForegroundColor = ConsoleColor.Black;
                                    BackgroundColor = AVAILABLE;
                                }
                                for (int j = 0; j < width; j++)
                                {
                                    if (i == (height - 1) / 2 && j == (width - 1) / 2)
                                    {
                                        // Token section
                                        if (Position[index - 1].isOccupied())
                                        {
                                            Token t = Position[index - 1].getToken();
                                            Player p = t.getPlayer();
                                            char symbol = p.getSymbol();
                                            Write(symbol);
                                        }
                                        else
                                        {
                                            Write(index);
                                        }
                                    }
                                    else
                                    {
                                        Write(PLACEHOLDER);
                                    }
                                }
                            }
                            ResetColor();
                            x += width;
                        }
                    }
                    y += height;
                }

                WriteLine(); // Change Line
            }


        }

        public void victoryRender()
        {
            Clear();
            int r = Row * 2 + 1;
            int c = Col * 2 + 1;
            int x, y = 0;
            int width = BOXSIZE * 2 + 1, height = BOXSIZE;


            for (int ci = 0; ci < c; ci++) // Each column line
            {
                x = 0;

                if (ci % 2 == 0)
                {
                    // Draw Line

                    for (int ri = 0; ri < r; ri++)
                    {
                        if (ri % 2 == 0)
                        {
                            // Intersection
                            Write(INTERSECTION);
                            x++;
                        }
                        else
                        {
                            // BoxLine
                            for (int i = 0; i < width; i++)
                            {
                                Write(H_LINE);
                                x++;
                            }
                        }
                    }
                    y++;
                }
                else
                {
                    // Draw row

                    for (int ri = 0; ri < r; ri++)
                    {
                        SetCursorPosition(x, y);
                        if (ri % 2 == 0)
                        {
                            // Vertical Line

                            for (int i = 0; i < height; i++)
                            {
                                SetCursorPosition(x, y + i);
                                Write(V_LINE);
                            }
                            x++;
                        }
                        else
                        {
                            // Box

                            int index = ci / 2 * Row + ri / 2 + ri % 2;
                            for (int i = 0; i < height; i++)
                            {
                                SetCursorPosition(x, y + i);
                                if (Position[index - 1].getToken() != null && 
                                    Position[index - 1].getToken().isVictory()) // If victory token change color
                                {
                                    ForegroundColor = ConsoleColor.Black;
                                    BackgroundColor = WIN;
                                }
                                for (int j = 0; j < width; j++)
                                {

                                    if (i == (height - 1) / 2 && j == (width - 1) / 2)
                                    {
                                        // Token section
                                        if (Position[index - 1].isOccupied())
                                        {
                                            Token t = Position[index - 1].getToken();
                                            Player p = t.getPlayer();
                                            char symbol = p.getSymbol();

                                            Write(symbol);
                                        }
                                        else
                                        {
                                            Write(PLACEHOLDER);
                                        }
                                    }
                                    else
                                    {
                                        Write(PLACEHOLDER);
                                    }
                                }
                            }
                            ResetColor();
                            x += width;
                        }
                    }
                    y += height;
                }
                WriteLine(); // Change Line
            }
        }
    }
}
