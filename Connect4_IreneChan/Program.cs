using System;
using System.Collections.Generic;

namespace Connect4_IreneChan
{
    // The Game Controller Class to play the game
    public class Connect4Controller
    {
        public static int BoardRow = 6;
        public static int BoardCol = 7;
        private static List<Player> _playerList;
        private static int _currentTurn;

        public Connect4Controller()
        {
            _playerList= new List<Player>();
            _currentTurn = 0;

            /*
            playerList = new List<Player>();
            var HP = new HumanPlayer();
            var CP = new ComputerPlayer();

            playerList.Add(HP);
            playerList.Add(CP);
            */
        }

        public int NextTurn()
        {
            // As a Connect 4 game will only have 2 players,
            // the current turn will only be switching between 0 and 1 (the 1st player and the 2nd player)
            if (_currentTurn == 0) return 1;
            else return 0;
        }

    }

    // Player Abstract (the derived class should communicate with the world via an object that is created in the controller class)
    abstract class Player
    {
        public string Name { get; set; }

        public abstract void Play();
    }

    // The Human Player Class from Player Abstract
    internal class HumanPlayer : Player
    {
        public override void Play()
        {
            
        }
    }

    // The Computer Player Class from Player Abstract
    internal class ComputerPlayer : Player
    {
        public override void Play()
        {

        }
    }

    // The Game Status Class (Model) for intermediate steps, holding information of the game
    internal class GameStatus
    {
        // The Connect 4 grids
        public static int[,] GameMap;

        static GameStatus()
        {
            // To initialize the grids
            /*
            // For actual implementation
            GameMap = new int[Connect4.BoardRow, Connect4.BoardCol];

            for(int i = 0; i < Connect4.BoardRow; i++)
            {
                for(int j = 0; j < Connect4.BoardCol; j++)
                {
                    GameMap[i, j] = 0;
                }
            }
            */

            // For Testing
            GameMap = new int[6, 7] 
            {
                { 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 0 }
            };
            
        }

        public static bool IsWin(int player, int lastStepX, int lastStepY)
        {
            // To check if the player is win or not. Return true if the player win. Otherwise, return false
            if (CheckVertical(player, lastStepX, lastStepY)) return true;
            else if (CheckHorizontal(player, lastStepX, lastStepY)) return true;
            else if (CheckDiagonal1(player, lastStepX, lastStepY)) return true;
            else if (CheckDiagonal2(player, lastStepX, lastStepY)) return true;

            return false;
        }

        private static bool CheckVertical(int player, int lastStepX, int lastStepY)
        {
            // The method is completed!!!

            // Check the vertical condition of last step
            int[,] gameMap = GameStatus.GameMap;
            int count = 1;

            // Check upper direction
            for(int i = 1; i < Connect4Controller.BoardRow - lastStepY; i++)
            {
                if (gameMap[lastStepY + i, lastStepX] == player) count++;
            }

            // Check lower direction
            for (int i = 1; i <= lastStepY; i++)
            {
                if (gameMap[lastStepY - i, lastStepX] == player) count++;
            }

            if (count >= 4) return true;

            return false;
        }

        private static bool CheckHorizontal(int player, int lastStepX, int lastStepY)
        {
            // The method is completed!!!

            // Check the horizontal condition of last step
            int[,] gameMap = GameStatus.GameMap;
            int count = 1;

            // Check right direction
            for (int i = 1; i < Connect4Controller.BoardCol - lastStepX; i++)
            {
                if (gameMap[lastStepY, lastStepX + i] == player) count++;
            }

            // Check left direction
            for (int i = 1; i <= lastStepX; i++)
            {
                if (gameMap[lastStepY, lastStepX - i] == player) count++;
            }

            if (count >= 4) return true;

            return false;
        }

        private static bool CheckDiagonal1(int player, int lastStepX, int lastStepY)
        {
            // Check the first diagonal condition of last step

            return false;
        }

        private static bool CheckDiagonal2(int player, int lastStepX, int lastStepY)
        {
            // Check the second diagonal condition of last step

            return false;
        }

    }

    // Screen Class for interaction in the console to get input data / display msg
    internal class Screen
    {
        public static void PrintGrids()
        {
            // The method is completed!!!

            int[,] gameMap = GameStatus.GameMap;
            Console.Write("\n");
            for (int i = 0; i < gameMap.GetLength(0); i++)
            {
                Console.Write("|");
                for (int j = 0; j < gameMap.GetLength(1); j++)
                {
                    if (gameMap[i, j] == 1) Console.Write("  X");
                    else if(gameMap[i, j] == 2) Console.Write("  O");
                    else Console.Write("  #");
                }
                Console.WriteLine("  |");
            }
            Console.WriteLine("   1  2  3  4  5  6  7\n");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Screen.PrintGrids();
            if(GameStatus.IsWin(1, 5, 5)) Console.WriteLine("Win!!!");
        }
    }
}
