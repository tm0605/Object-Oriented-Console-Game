using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using static IFN563_Assignment.Rule;

namespace IFN563_Assignment
{
    abstract class Player
    {
        protected string Name { get; set; }
        protected int PlayerNo { get; set; }
        protected char Symbol { get; set; }
        protected const ConsoleColor ERROR = ConsoleColor.Red;
        public Player(string name, int no, char symbol)
        {
            Name = name;
            PlayerNo = no;
            Symbol = symbol;
        }
        public string getName()
        {
            return Name;
        }
        public char getSymbol()
        {
            return Symbol;
        }
        public abstract string promptMove();
    }

    class Human : Player
    {
        public Human(string name, int no, char symbol) : base(name, no, symbol)
        {

        }
        public override string promptMove()
        {
            string input;
            int index;
            bool isValid;
            do
            {
                WriteLine();
                input = ReadLine();


                if (input == Game.UNDO || input == Game.REDO || input == Game.SAVE || input == Game.HELP)
                {
                    return input;
                }
                else if (!int.TryParse(input, out index))
                {
                    ForegroundColor = ERROR;
                    WriteLine("INVALID INPUT\nEnter a valid input");
                    isValid = false;
                    ResetColor();
                }
                else if (index >= 1 && index <= Game.ROW * Game.COL)
                {
                    input = index.ToString();
                    isValid = true;
                }
                else
                {
                    ForegroundColor = ERROR;
                    WriteLine("INVALID INPUT\nEnter an integer between 1 - {0}", Game.ROW * Game.COL);
                    isValid = false;
                    ResetColor();
                }
            }
            while (!isValid);

            return input;
        }
    }

    class Computer : Player
    {
        // private int Difficulty { get; set; }
        public Computer(string name, int no, char symbol) : base(name, no, symbol)
        {

        }
        public override string promptMove()
        {
            Random random = new Random();

            int index = random.Next(1, Game.ROW * Game.COL);
            
            return index.ToString();
        }
    }
}
