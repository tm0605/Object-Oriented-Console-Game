using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static System.Console;
using static IFN563_Assignment.Rule;

namespace IFN563_Assignment
{
    internal class Game
    {
        int currentTurn;
        protected Player[] Players { get; set; }
        protected Token[] Tokens { get; set; }
        protected Board Board { get; set; }
        protected Player CurrentPlayer { get; set; }
        protected int CurrentTurn
        {
            get { return currentTurn; }
            set { currentTurn = value; setState(); }
        }
        private bool GameOver { get; set; }
        protected int[] UndoTokenIndex { get; set; }
        protected int UndoCount { get; set; }
        private void setState()
        {
            if (isWinningMove(Board.getPositions()) || isDraw(Board.getPositions()))
                GameOver = true;
        }

        // Game Settings
        public const int ROW = 3, COL = 3;
        public const string UNDO = "U", REDO = "R", SAVE = "S", CONTINUE = "C", HELP = "H";
        public const string FILENAME = "/savefile.txt";
        const ConsoleColor ERROR = ConsoleColor.Red;
        public Game(Player[] players)
        {
            // Game setup that applies to both new and saved games
            Players = players;

            Board board = new Board(ROW, COL);
            Board = board;

            Token[] tokens = new Token[ROW * COL];
            Tokens = tokens;

            CurrentTurn = 1;
        }
        public void run()
        {
            UndoTokenIndex = new int[ROW * COL];
            UndoCount = 0;

            Board.render();// First render of the board


            while (!GameOver) // GAME LOOP while its not a winning move and not a draw
            {
                assignPlayer(); // Change current player

                printInstruction(); // Message on the console to direct the player

                promptMove(); // Prompt move from player
            }


            if (isWinningMove(Board.getPositions())) // If player wins
            {
                Board.victoryRender();
                WriteLine("Winner: {0} ({1})", CurrentPlayer.getName(), CurrentPlayer.getSymbol());
            }
            else if (isDraw(Board.getPositions())) // If it's a draw
            {
                WriteLine("It's a draw!");
            }

            WriteLine("Press any key to continue...");
            ReadKey();
        }
        private void saveGame()
        {
            string player_str = "";

            for (int i = 0; i < Players.Length; i++) // Save player data
            {
                if (Players[i].GetType() == typeof(Human)) // Player type
                {
                    player_str += "H";
                }
                else
                {
                    player_str += "C";
                }
                player_str += ",";
                player_str += Players[i].getName(); // Record player name
                player_str += ",";
                player_str += i + 1; // Record player No
                player_str += ",";
                player_str += Players[i].getSymbol(); // Record player Symbol

                if (i != Players.Length - 1) // If not last player add space
                {
                    player_str += " ";
                }
                else // If last player add new line
                {
                    player_str += "\r\n";
                }
            }

            string token_str = "";

            for (int i = 0; i < CurrentTurn - 1; i++) // Save token data
            {
                token_str += Tokens[i].getTokenNo(); // Record token No
                token_str += ",";

                for (int j = 0; j < Players.Length; j++) // Record token owner
                {
                    if (Tokens[i].getPlayer() == Players[j])
                        token_str += j + 1;
                }
                
                if (i != CurrentTurn - 2) // If not last token add space
                {
                    token_str += " ";
                }
            }
            token_str += "\r\n"; // Add a new line at the end

            string position_str = "";
            Position[] posArr = Board.getPositions();
            for (int i = 0; i < posArr.Length; i++) // Save position data
            {
                position_str += posArr[i].getIndex(); // Record position index
                position_str += ",";
                Token t = posArr[i].getToken();
                if (t != null) // If token exists in position
                {
                    position_str += t.getTokenNo(); // Record token No
                }
                else
                {
                    position_str += "null";
                }
                position_str += ",";
                position_str += posArr[i].isOccupied().ToString(); // Record position occupancy
                
                if (i != posArr.Length - 1) // If not last position
                {
                    position_str += " ";
                }
            }
            
            string output = player_str + token_str + position_str; // Combine string

            try
            {
                string path = Directory.GetCurrentDirectory();
                File.WriteAllText(path + FILENAME, output);
                WriteLine("Game saved!");
            }
            catch (Exception e) // If failed to save
            {
                WriteLine("Failed to save game");
                WriteLine(e);
            }
        }
        private void undo()
        {
            Token currentToken = Tokens[CurrentTurn - 2];

            // Removes the token from Position object but will keep the token in Tokens array as it can be overwritten when need to
            foreach (Position p in Board.getPositions()) // Search for position that has the token
            {
                if (p.getToken() == currentToken) // Remove token from position
                {
                    UndoTokenIndex[UndoCount] = p.getIndex(); // Record the index for redo function
                    p.removeToken();
                    break;
                }
            }

            UndoCount++; // Increment UndoCount so that redo function will be available
            CurrentTurn--; // Decrement Turn so that Tokens array can be overwritten

        }
        private void redo()
        {
            Position p = Board.getPosition(UndoTokenIndex[UndoCount - 1]); // Retrieve position using the index stored
            p.setToken(Tokens[CurrentTurn - 1]); // Re-set the token to position

            UndoCount--; // Decrement UndoCount
            CurrentTurn++; // Increment Turn
        }
        private void help()
        {
            Clear();
            WriteLine("TIC TAC TOE RULES");
            WriteLine("The game is played on a grid that's 3 squares by 3 squares");
            WriteLine("The first player to get 3 makrs in a row, either up, down, across or diagonally will be the winner");
            WriteLine("When all 9 squarese are full, the game is over and will end in a tie");
            WriteLine("Press any key to continue...");
            ReadKey();
        }
        private void promptMove()
        {
            bool isValid;
            do
            {
                string input;
                int index;

                // Prompting Move
                if (CurrentPlayer.GetType() == typeof(Computer) && UndoCount > 0) // If undo executed in PvC
                {
                    bool isValid2;
                    do
                    {
                        // To prevent not being able to undo more than one during PvC
                        input = ReadLine(); // Ask human player for next command before computer executes next move
                        if (input == CONTINUE) // If human player decides for the computer to make the move
                        {
                            input = CurrentPlayer.promptMove(); // Prompt Com move
                            isValid2 = true;
                        }
                        else if (input == SAVE || input == UNDO || input == REDO || input == HELP) // If undo more or redo
                        {
                            isValid2 = true;
                        }
                        else
                        {
                            ForegroundColor = ERROR;
                            isValid2 = false;
                            WriteLine("Invalid Input");
                            ResetColor();
                        }
                    }
                    while (!isValid2);
                }
                else // Normal Procedure
                {
                    input = CurrentPlayer.promptMove(); // Prompt move
                }


                // Process the Input
                switch (input)
                {
                    case UNDO: // If undo command
                        if (CurrentTurn > 1) // If there is available commands to undo
                        {
                            isValid = true;
                            undo(); // Perform undo method
                            Board.render();
                        }
                        else // If no commands to undo
                        {
                            ForegroundColor = ERROR;
                            isValid = false;
                            WriteLine("No commands to undo");
                            ResetColor();
                        }
                        break;

                    case REDO: // If redo command
                        if (UndoCount > 0) // if there is available commands to redo
                        {
                            isValid = true;
                            redo(); // Perform redo method
                            Board.render();
                        }
                        else // If no commands to redo
                        {
                            ForegroundColor = ERROR;
                            isValid = false;
                            WriteLine("No command to redo");
                            ResetColor();
                        }
                        break;

                    case SAVE: // If save command
                        isValid = true;
                        saveGame(); // Perform save method
                        GameOver = true;
                        break;

                    case HELP: // If help command
                        isValid = true;
                        help();
                        Board.render();
                        break;

                    default: // If normal command
                        index = int.Parse(input); // Change the variable type, No error will occur as it's been checked in the promptMove() method

                        // Check the position vacancy
                        if (Rule.isValid(Board.getPosition(index))) // If position vacant
                        {
                            isValid = true;

                            createToken(index);

                            UndoCount = 0; // Reset Undo Count to make redo unavailable
                            CurrentTurn++; // Increment CurrentTurn

                            Board.render();
                        }
                        else if (CurrentPlayer.GetType() == typeof(Human)) // If position occupied for human
                        {
                            ForegroundColor = ERROR;
                            isValid = false;
                            WriteLine("Invalid Input\nPosition Occupied");
                            ResetColor();
                        }
                        else // If position occupied for COM
                        {
                            isValid = false;
                        }
                        break;
                }
            }
            while (!isValid);
        }
        protected void assignPlayer()
        {
            int n = (CurrentTurn - 1) % Players.Length;
            CurrentPlayer = Players[n];
        }
        private void printInstruction()
        {
            if (CurrentPlayer.GetType() == typeof(Computer) && UndoCount > 0) // Special message when human player undo during a PvC mode
            {
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine("\nBefore {0} makes it's next move...", CurrentPlayer.getName());

                WriteLine("\"{5}\" for help, \"{0}\" to save, \"{1}\" to undo more enter, \"{2}\" to redo\nOr \"{3}\" to let {4} make it's next move"
                    , SAVE, UNDO, REDO, CONTINUE, CurrentPlayer.getName(), HELP);
                ResetColor();
            }
            else if (CurrentPlayer.GetType() == typeof(Human)) // Normal Message for human player
            {
                WriteLine("\n{0}'s Turn ({1})", CurrentPlayer.getName(), CurrentPlayer.getSymbol());

                string baseMessage = "Enter a number or \"" + HELP + "\" for help";
                string saveMessage = ", \"" + SAVE + "\" to save";
                string undoMessage = ", \"" + UNDO + "\" to undo";
                string redoMessage = ", \"" + REDO + "\" to redo";
                if (CurrentTurn == 1 && UndoCount == 0) // First turn
                {
                    WriteLine(baseMessage);
                }
                else if (UndoCount > 0 && CurrentTurn == 1) // Redo available but undo unavailable
                {
                    WriteLine(baseMessage + saveMessage + redoMessage);
                }
                else if (UndoCount > 0) // Undo redo available
                {
                    WriteLine(baseMessage + saveMessage + undoMessage + redoMessage);
                }
                else // Undo available
                {
                    WriteLine(baseMessage + saveMessage + undoMessage);
                }
            }
        }
        private void createToken(int index)
        {
            Tokens[CurrentTurn - 1] = new Token(CurrentTurn, CurrentPlayer); // Create Token
            Position p = Board.getPosition(index);
            p.setToken(Tokens[CurrentTurn - 1]); // Set token to position
        }
    }
    class NewGame : Game
    {
        public NewGame(Player[] players) : base(players)
        {

        }
    }

    class SavedGame : Game
    {
        public SavedGame(Player[] players) : base(players)
        {
            string readText = "";

            try
            {
                string path = Directory.GetCurrentDirectory();
                readText = File.ReadAllText(path + FILENAME);
            }
            catch (Exception e) // If file does not exist in directory DOESNT WORK!!!!!!!!!!!!!!!
            {
                WriteLine(e.Message);
                WriteLine("Could not find the file");
            }

            try
            {
                string[] lines = readText.Split("\r\n"); // Split string by new line
                object[] data = new object[lines.Length]; // Array of a 2d array extracted data

                for (int i = 0; i < data.Length; i++) // Create each 2d array
                {
                    string[] obj = lines[i].Split(" "); // Split by ojects
                    string[,] arr = new string[obj.Length, obj[0].Split(",").Length]; // Declare 2d array row = number of objects col = number of paramters

                    for (int l = 0; l < obj.Length; l++) // for each object
                    {
                        string[] par = obj[l].Split(",");

                        for (int k = 0; k < par.Length; k++) // for each parameter in an object
                        {
                            arr[l, k] = par[k];
                        }
                    }
                    data[i] = arr;
                }

                for (int i = 1; i < data.Length; i++) // Set up game
                {
                    switch (i)
                    {
                        case 1: // Set up tokens
                            string[,] tokens = (string[,])data[i];
                            if (tokens[0, 0] == "")
                                break;

                            for (int j = 0; j < tokens.GetLength(0); j++)
                            {
                                assignPlayer();
                                Tokens[CurrentTurn - 1] = new Token(CurrentTurn, CurrentPlayer); // Create token
                                CurrentTurn++;
                            }
                            break;

                        case 2: // Set positions data
                            string[,] positions = (string[,])data[i];

                            for (int l = 0; l < positions.GetLength(0); l++)
                            {
                                if (bool.Parse(positions[l, 2]) == true)
                                {
                                    Position p = Board.getPosition(int.Parse(positions[l, 0]));
                                    foreach (Token t in Tokens)
                                    {
                                        if (t.getTokenNo() == int.Parse(positions[l, 1])) // If the token no matches
                                        {
                                            p.setToken(t); // set token to position
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
                WriteLine("Could not load game");
            }
        }
    }
}
