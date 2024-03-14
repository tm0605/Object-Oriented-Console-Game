using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static System.Console;

namespace IFN563_Assignment
{
    abstract class Creator
    {

        public static Game initialize()
        {
            Clear();
            string title = @"
        ,----,                                 ,----,                                      ,----,                     
      ,/   .`|                               ,/   .`|                                    ,/   .`| ,----..             
    ,`   .'  :  ,---, ,----..              ,`   .'  :,---,        ,----..              ,`   .'  :/   /   \     ,---,. 
  ;    ;     ,`--.' |/   /   \           ;    ;     '  .' \      /   /   \           ;    ;     /   .     :  ,'  .' | 
.'___,/    ,'|   :  |   :     :        .'___,/    ,/  ;    '.   |   :     :        .'___,/    ,.   /   ;.  ,---.'   | 
|    :     | :   |  .   |  ;. /        |    :     :  :       \  .   |  ;. /        |    :     .   ;   /  ` |   |   .' 
;    |.';  ; |   :  .   ; /--`         ;    |.';  :  |   /\   \ .   ; /--`         ;    |.';  ;   |  ; \ ; :   :  |-, 
`----'  |  | '   '  ;   | ;            `----'  |  |  :  ' ;.   :;   | ;            `----'  |  |   :  | ; | :   |  ;/| 
    '   :  ; |   |  |   : |                '   :  |  |  ;/  \   |   : |                '   :  .   |  ' ' ' |   :   .' 
    |   |  ' '   :  .   | '___             |   |  '  :  | \  \ ,.   | '___             |   |  '   ;  \; /  |   |  |-, 
    '   :  | |   |  '   ; : .'|            '   :  |  |  '  '--' '   ; : .'|            '   :  |\   \  ',  /'   :  ;/| 
    ;   |.'  '   :  '   | '/  :            ;   |.'|  :  :       '   | '/  :            ;   |.'  ;   :    / |   |    \ 
    '---'    ;   |.'|   :    /             '---'  |  | ,'       |   :    /             '---'     \   \ .'  |   :   .' 
             '---'   \   \ .'                     `--''          \   \ .'                         `---`    |   | ,'   
                      `---`                                       `---`                                    `----'     
                                                                                                                      
";
            string instruction = "\n\nWelcome to Tic Tac Toe\n\nChoose one from the following\n";
            string[] options = { "New Game", "Continue", "Quit" };

            Menu openingMenu = new Menu(instruction, options);
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(title);
            ResetColor();
            int choice = openingMenu.displayMenu(); // Implement error funciton!!!!

            switch (choice)
            {
                case 1:
                    Creator c = new NewGame_Creator();
                    return c.createGame();
                case 2:
                    c = new SavedGame_Creator();
                    return c.createGame();
                case 3:
                    return null;
                default:
                    return null;
            }
        }
        public abstract Game createGame();
    }
    class NewGame_Creator : Creator
    {
        public override Game createGame()
        {
            Clear();
            string message = "Choose one from the following\n";
            string[] options = { "PvC", "PvP" };


            Menu modeMenu = new Menu(message, options);
            int choice = modeMenu.displayMenu();


            if (choice == 1) // PvC
            {
                Player[] players = new Player[2];

                Write("Enter P1 name: ");
                string name = ReadLine();
                players[0] = new Human(name, 1, 'X'); // Assign human

                players[1] = new Computer("COM", 2, 'O'); // Assign Computer

                NewGame game = new NewGame(players);
                return game;
            }
            else // PvP
            {
                Human[] players = new Human[2];

                for (int i = 0; i < players.Length; i++)
                {
                    Write("Enter P{0} name: ", i + 1);
                    string name = ReadLine();
                    char symbol = 'X';
                    if (i == 1)
                        symbol = 'O';

                    players[i] = new Human(name, i + 1, symbol);
                    WriteLine();
                }

                NewGame game = new NewGame(players);
                return game;
            }
        }
    }
    class SavedGame_Creator : Creator
    {
        public override Game createGame()
        {
            string readText;
            Player[] players = new Player[2];

            try
            {
                string path = Directory.GetCurrentDirectory();
                readText = File.ReadAllText(path + Game.FILENAME);
            }
            catch (Exception e)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine(e.Message);
                ReadKey();
                ResetColor();
                return null;
            }

            try
            {
                string[] lines = readText.Split("\r\n"); // Split string by new line

                string[] obj = lines[0].Split(" "); // Split by ojects
                string[,] arr = new string[obj.Length, obj[0].Split(",").Length]; // Declare 2d array row = number of objects col = number of paramters

                for (int l = 0; l < obj.Length; l++) // for each object
                {
                    string[] par = obj[l].Split(",");

                    for (int k = 0; k < par.Length; k++) // for each parameter in an object
                    {
                        arr[l, k] = par[k];
                    }
                }

                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    string name = arr[i, 1];
                    int p_no = int.Parse(arr[i, 2]);
                    char symbol = char.Parse(arr[i, 3]);

                    if (arr[i, 0] == "H") // Human
                    {
                        players[i] = new Human(name, p_no, symbol);
                    }
                    else if (arr[i, 0] == "C")
                    {
                        players[i] = new Computer(name, p_no, symbol);
                    }
                }
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Could not load game");
                ReadKey();
                ResetColor();

                return null;
            }

            SavedGame game = new SavedGame(players);
            return game;
        }
    }
}
