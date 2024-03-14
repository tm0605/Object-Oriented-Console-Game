using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace IFN563_Assignment
{
    internal class Menu
    {
        private string Message { get; set; }
        private string[] Options { get; set; }
        const ConsoleColor ERROR = ConsoleColor.Red;
        public Menu(string message, string[] options)
        {
            Message = message;
            Options = options;
        }

        public int displayMenu()
        {
            string input;
            int choice;
            bool isValid;

            WriteLine(Message);
            for (int i = 0; i < Options.Length; i++)
            {
                WriteLine(i + 1 + ". " + Options[i]);
            }
            WriteLine();
            do
            {
                input = ReadLine();

                if (!int.TryParse(input, out choice))
                {
                    ForegroundColor = ERROR;
                    WriteLine("INVALID INPUT\nEnter an integer\n");
                    isValid = false;
                    ResetColor();
                }
                else if (choice >= 1 && choice <= Options.Length)
                {
                    isValid = true;
                }
                else
                {
                    ForegroundColor = ERROR;
                    WriteLine("INVALID INPUT\nEnter an integer between 1 - {0}\n", Options.Length);
                    isValid = false;
                    ResetColor();
                }
            }
            while (!isValid);
            return choice;
        }
    }
}
