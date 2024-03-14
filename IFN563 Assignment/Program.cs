using System;
using System.IO;
using static System.Console;

namespace IFN563_Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game g = Creator.initialize();
            if (g != null)
                g.run();

            Clear();
            WriteLine("Thanks for playing!\nSee you!");
        }
    }
}
