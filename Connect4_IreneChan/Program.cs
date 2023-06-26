using System;
using System.Collections.Generic;

namespace Connect4_IreneChan
{
    /*********************************************************************
     * Connect4Controller Class
     * Description: The Game Controller Class to play the Connect 4 game
     *********************************************************************/
    public class Connect4Controller
    {
        public static int BoardRow = 6;             // To define the game board row
        public static int BoardCol = 7;             // To define the game board column
        private static List<Player> _playerList;    // The player list
        private static int _currentTurn;            // To store the current turn
        private static int _count;                  // To store the current no. of turns
        private Random r;                           // To randomly assign the players' sequence
        private Screen _screen;                     // The Screen object to display the game board
        private GameStatus _gameStatus;             // The GameStatus object to store the game status

        static Connect4Controller()
        {
            // To initialize the static fields
            _playerList= new List<Player>();
            _currentTurn = 0;
            _count = 0;
        }

        public Connect4Controller()
        {
            r = new Random();
            _screen = new Screen();
            _gameStatus = new GameStatus();
        }

        public void PlayConnect4()
        {
            Console.Clear();

            // Ask the player to choose the game mode
            ChoosingGameMode();

            // Randomly assign the players' sequence
            RandomPlayersSeq();

            // Display the sequence and players' name
            DisplayPlayers();

            Console.WriteLine("\nPress [Enter] to start the game... ");
            string start = Console.ReadLine();

            // The main game logic
            while (true)
            {
                PrintGameHeader();

                Console.WriteLine($"It is {_playerList[_currentTurn].Name}'s turn! ");

                // Get the x coordinate of the next move position from the player
                int moveXInput = _playerList[_currentTurn].Play();
                int moveYCal = -1;

                // Error handling when input is out of range
                try
                {
                    if (moveXInput < 0 || moveXInput >= 7) throw new OutOfRangeException("Please enter 1 to 7... ");
                }
                catch (OutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("\nPress [Enter] to continue... ");
                    start = Console.ReadLine();
                    continue;
                }

                // Calculate the y coordination from the game map with the x coordinate
                for (int i = Connect4Controller.BoardRow - 1; i >= 0; i--)
                {
                    if (_gameStatus.GameMap[i, moveXInput] == -1)
                    {
                        moveYCal = i;
                        _gameStatus.GameMap[i, moveXInput] = _currentTurn;
                        break;
                    }
                }

                if (moveYCal == -1)
                {
                    // If no valid value is assigned to the y coordinate, 
                    // i.e. the column is full. 
                    // Ask the player to enter the other column number
                    Console.WriteLine("The column is full. Please enter another column... ");
                    Console.WriteLine("\nPress [Enter] to continue... ");
                    start = Console.ReadLine();
                    continue;
                }

                // After gettting the valid move, check if the player is win
                if (_gameStatus.IsWin(_currentTurn, moveXInput, moveYCal))
                {
                    PrintGameHeader();
                    Console.WriteLine($"{_playerList[_currentTurn].Name} is Win!!!");
                    break;
                }else if(_count >= 41)
                {
                    // If the current turn count is over 41 and no player wins, it is a draw
                    PrintGameHeader();
                    Console.WriteLine("It is a draw");
                    break;
                }

                NextTurn();
            }

            // Ask if the player wants to restart the game
            string restart = "";
            while (true)
            {
                Console.Write("Restart? [Y/N]: ");
                restart = Console.ReadLine().ToUpper();
                if(restart == "Y")
                {
                    RestartGame();
                    break;
                }else if(restart == "N")
                {
                    break;
                }
            }
        }

        private void ChoosingGameMode()
        {
            // Choosing the number of players from the player (1 or 2)
            Console.WriteLine("Enter the number of player(s) [ 1 or 2 ]: ");
            int noOfPlayers;

            while (true)
            {
                noOfPlayers = int.Parse(Console.ReadLine());
                if (noOfPlayers == 1 || noOfPlayers == 2)
                {
                    CreatePlayerList(noOfPlayers);
                    break;
                }
                else
                {
                    Console.WriteLine("The number of player(s) should be 1 or 2: ");
                }
            }
        }

        private void CreatePlayerList(int noOfPlayers)
        {
            // Create the player list based on player's input
            if(noOfPlayers == 2)
            {
                for(int i = 0; i < noOfPlayers; i++)
                {
                    _playerList.Add(new HumanPlayer(InputPlayer(i)));
                }
            }else if (noOfPlayers == 1)
            {
                _playerList.Add(new HumanPlayer(InputPlayer()));
                _playerList.Add(new ComputerPlayer());
            }
        }

        // Overloading by Default Arguments
        private string InputPlayer(int seq = 0)
        {
            if (seq == 0) Console.WriteLine("Enter the 1st player's name: ");
            else if (seq == 1) Console.WriteLine("Enter the 2nd player's name: ");

            return Console.ReadLine();
        }

        private void RandomPlayersSeq()
        {
            // Randomly assign the sequence of the players
            if(r.Next(0, 2) == 0) _playerList.Reverse();
        }

        private void DisplayPlayers()
        {
            for(int i = 0; i < _playerList.Count; i++)
            {
                Console.WriteLine($"Player {i+1} is {_playerList[i].Name}");
            }
        }

        private void PrintGameHeader()
        {
            Console.Clear();
            Console.WriteLine($"\n\nIt is a Connect 4 Game! ");
            _screen.PrintGrids(_gameStatus);
        }

        private void RestartGame()
        {
            // Restart the game,
            // clear all the game data for the previous game
            // and start the game again
            _playerList.Clear();
            _currentTurn = 0;
            _count = 0;

            for (int i = 0; i < Connect4Controller.BoardRow; i++)
            {
                for (int j = 0; j < Connect4Controller.BoardCol; j++)
                {
                    _gameStatus.GameMap[i, j] = -1;
                }
            }

            PlayConnect4();
        }

        private void NextTurn()
        {
            // As a Connect 4 game will only have 2 players,
            // the current turn will only be switching between 0 and 1 (the 1st player and the 2nd player)
            if (_currentTurn == 0) _currentTurn = 1;
            else _currentTurn = 0;
            _count++;
        }

    }

    /*********************************************************************
     * Player Abstract
     * Description: The base class of a player with Name property
     *              and an abstract method Play() to play the game 
     *              (make the next move)
     *********************************************************************/
    abstract class Player
    {
        public string Name { get; set; }
        protected Random r;

        public Player(string name)
        {
            Name = name;
            r = new Random();
        }

        public abstract int Play();
    }

    /*********************************************************************
     * HumanPlayer Class
     * Description: Inherited from Player Abstract, 
     *              to implement the Play() method as a human player
     *********************************************************************/
    internal class HumanPlayer : Player
    {
        public HumanPlayer(string name) : base(name)
        {

        }

        public override int Play()
        {
            // Read and return the input move from the player
            Console.Write("Please make your move [Enter 1 to 7]: ");
            int num = -1;
            string input = Console.ReadLine();
            if(input == null || input == "" || !int.TryParse(input, out num)) return -1;
            return num - 1;
        }
    }

    /*********************************************************************
     * ComputerPlayer Class
     * Description: Inherited from Player Abstract, 
     *              to implement the Play() method as a computer player
     *********************************************************************/
    internal class ComputerPlayer : Player
    {
        public ComputerPlayer() : base("Computer")
        {
            
        }

        public override int Play()
        {
            Console.WriteLine("It is the computer to make the move! ");
            Console.WriteLine("\nPress [Enter] to continue... ");
            string start = Console.ReadLine();

            // A random move from the computer
            return r.Next(0, 7);
        }
    }

    /*********************************************************************
     * GameStatus Class
     * Description: The Game Status Class (Model) for intermediate steps, 
     *              holding information of the game. It also contains a 
     *              IsWin() method to check if the winner has win the game
     *********************************************************************/
    internal class GameStatus
    {
        // The Connect 4 grids
        public int[,] GameMap { get; set; }

        public GameStatus()
        {
            // To initialize the grids
            GameMap = new int[Connect4Controller.BoardRow, Connect4Controller.BoardCol];

            for(int i = 0; i < Connect4Controller.BoardRow; i++)
            {
                for(int j = 0; j < Connect4Controller.BoardCol; j++)
                {
                    GameMap[i, j] = -1;
                }
            }
        }

        public bool IsWin(int player, int lastStepX, int lastStepY)
        {
            // To check if the player is win or not. Return true if the player win. Otherwise, return false
            if (CheckVertical(player, lastStepX, lastStepY)) return true;
            else if (CheckHorizontal(player, lastStepX, lastStepY)) return true;
            else if (CheckDiagonal1(player, lastStepX, lastStepY)) return true;
            else if (CheckDiagonal2(player, lastStepX, lastStepY)) return true;

            return false;
        }

        private bool CheckVertical(int player, int lastStepX, int lastStepY)
        {
            // Check the vertical condition of last step
            int count = 1;

            // Check upper direction
            for (int i = 1; i <= lastStepY; i++)
            {
                if (GameMap[lastStepY - i, lastStepX] == player) count++;
                else break;
            }

            // Check lower direction
            for (int i = 1; i < Connect4Controller.BoardRow - lastStepY; i++)
            {
                if (GameMap[lastStepY + i, lastStepX] == player) count++;
                else break;
            }

            if (count >= 4) return true;

            return false;
        }

        private bool CheckHorizontal(int player, int lastStepX, int lastStepY)
        {
            // Check the horizontal condition of last step
            int count = 1;

            // Check right direction
            for (int i = 1; i < Connect4Controller.BoardCol - lastStepX; i++)
            {
                if (GameMap[lastStepY, lastStepX + i] == player) count++;
                else break;
            }

            // Check left direction
            for (int i = 1; i <= lastStepX; i++)
            {
                if (GameMap[lastStepY, lastStepX - i] == player) count++;
                else break;
            }

            if (count >= 4) return true;

            return false;
        }

        private bool CheckDiagonal1(int player, int lastStepX, int lastStepY)
        {
            // Check the first diagonal (from left bottom to right up) condition of last step
            int count = 1;
            int checkStepRightUp;
            int checkStepLeftBottom;

            // Check right up direction
            if (Connect4Controller.BoardCol - lastStepX < lastStepY) 
                checkStepRightUp = Connect4Controller.BoardCol - lastStepX;
            else 
                checkStepRightUp = lastStepY;

            for (int i = 1; i < checkStepRightUp; i++)
            {
                if (GameMap[lastStepY - i, lastStepX + i] == player) count++;
                else break;
            }

            // Check left bottom direction
            if (lastStepX < Connect4Controller.BoardRow - lastStepY)
                checkStepLeftBottom = lastStepX;
            else
                checkStepLeftBottom = Connect4Controller.BoardRow - lastStepY;

            for (int i = 1; i < checkStepLeftBottom; i++)
            {
                if (GameMap[lastStepY + i, lastStepX - i] == player) count++;
                else break;
            }

            if (count >= 4) return true;


            return false;
        }

        private bool CheckDiagonal2(int player, int lastStepX, int lastStepY)
        {
            // Check the second diagonal (from left up to right bottom) condition of last step
            int count = 1;
            int checkStepLeftUp;
            int checkStepRightBottom;

            // Check left up direction
            if (lastStepX < lastStepY)
                checkStepLeftUp = lastStepX;
            else
                checkStepLeftUp = lastStepY;

            for (int i = 1; i <= checkStepLeftUp; i++)
            {
                if (GameMap[lastStepY - i, lastStepX - i] == player) count++;
                else break;
            }

            // Check right bottom direction
            if (Connect4Controller.BoardCol - lastStepX < Connect4Controller.BoardRow - lastStepY)
                checkStepRightBottom = Connect4Controller.BoardCol - lastStepX;
            else
                checkStepRightBottom = Connect4Controller.BoardRow - lastStepY;

            for (int i = 1; i < checkStepRightBottom; i++)
            {
                if (GameMap[lastStepY + i, lastStepX + i] == player) count++;
                else break;
            }

            if (count >= 4) return true;

            return false;
        }
    }

    /*********************************************************************
     * IScreen Interface
     * Description: It is an interface for the implemented class 
     *              to print the game status map (the game board). 
     *              The interface provides the extendibility for the style 
     *              of displaying the game board, 
     *              e.g. using different shapes / colors of symbols. 
     *********************************************************************/
    interface IScreen
    {
        void PrintGrids(GameStatus gameStatus);
    }

    /*********************************************************************
     * Screen Class
     * Description: It implements the IScreen interface 
     *              to display the game board in the console
     *********************************************************************/
    internal class Screen : IScreen
    {
        // Printing the game board based on the game map from gameStatus
        public void PrintGrids(GameStatus gameStatus)
        {
            int[,] gameMap = gameStatus.GameMap;
            Console.Write("\n");
            for (int i = 0; i < gameMap.GetLength(0); i++)
            {
                Console.Write("|");
                for (int j = 0; j < gameMap.GetLength(1); j++)
                {
                    if (gameMap[i, j] == 0) Console.Write("  X");
                    else if(gameMap[i, j] == 1) Console.Write("  O");
                    else Console.Write("   ");
                }
                Console.WriteLine("  |");
            }
            Console.WriteLine("   1  2  3  4  5  6  7\n");
        }
    }

    /*********************************************************************
     * OutOfRangeException Class
     * Description: It inherited the ApplicationException class
     *              to handle the user input which is out of 
     *              the expected range (1 to 7)
     *********************************************************************/
    public class OutOfRangeException : ApplicationException
    {
        public OutOfRangeException(string msg) : base(msg) { }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Connect4Controller connect4Game = new Connect4Controller();
            connect4Game.PlayConnect4();
        }
    }
}
