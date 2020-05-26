using System;
using System.Collections.Generic;

namespace MemoryGame
{
    struct Interface
    {
        public static void StartGame()
        {
            Player                player1 = new Player();
            Player                player2 = new Player();
            eGameStyle            styleChoise;
            Dictionary<int, char> valueDictionary = new Dictionary<int, char>();

            System.Console.WriteLine("Please enter your name:");
            player1.Name = System.Console.ReadLine();
            styleChoise = getUserGameStyle();

            if (styleChoise == eGameStyle.PlayerVSPlayer)
            {
                System.Console.WriteLine("Please enter the second player name:");
                player2.Name = System.Console.ReadLine();
            }

            GetSizesInitAndPlay(valueDictionary, player1, player2, styleChoise);
        }

        private static void GetSizesInitAndPlay(Dictionary<int, char> i_ValueDictionary, Player i_Player1, Player i_Player2, eGameStyle i_GameStyle)
        {
            int rows = 0;
            int cols = 0;

            GetSizes(ref cols, ref rows);
            InitDictionary(i_ValueDictionary, cols, rows);

            if (i_GameStyle == eGameStyle.PlayerVSComputer)
            {
                PlayGameVSComputer(i_ValueDictionary, i_Player1, rows, cols);
            }
            else
            {
                PlayGameVSPlayer(i_ValueDictionary, i_Player1, i_Player2, rows, cols);
            }
        }

        private static eGameStyle getUserGameStyle()
        {
            int styleChoise;

            System.Console.WriteLine("If you wish to play against the computer, press 1.\nIf you wish to play against another player, press 2.");
            int.TryParse(System.Console.ReadLine(), out styleChoise);

            while ((eGameStyle)styleChoise != eGameStyle.PlayerVSComputer && (eGameStyle)styleChoise != eGameStyle.PlayerVSPlayer)
            {
                System.Console.WriteLine("You have entered invalid input, please try again (choose 1 or 2):");
                int.TryParse(System.Console.ReadLine(), out styleChoise);
            }

            return (eGameStyle)styleChoise;
        }

        private static bool AnotherGame()
        {
            string userInput = string.Empty;
            bool   choice = false;

            System.Console.WriteLine("If you wish to start another game press 'Y' else press any key.");
            userInput = System.Console.ReadLine();

            if (userInput == 'Y'.ToString())
            {
                choice = true;
            }

            return choice;
        }
        
        private static void PlayGameVSComputer(Dictionary<int, char> i_ValueDictionary, Player i_Player1, int i_Rows, int i_Cols)
        {
            GameBoard  board = new GameBoard(i_Rows, i_Cols);
            Computer   computer = new Computer(i_Rows, i_Cols);
            bool       isTurnKept = false;
            bool       player1Turn = true;
            string     msgToUser = string.Empty;
            Player     player2 = null;
            eGameStyle gameStyle = eGameStyle.PlayerVSComputer;

            while (board.IsGameWon() == false)
            {
                if (player1Turn == true)
                {
                    msgToUser = string.Format("{0} has the turn.", i_Player1.Name);
                    ClearPrintMsg(board, i_ValueDictionary, msgToUser);
                    System.Console.Write("For first tile, ");
                    isTurnKept = PlayerTurn(i_ValueDictionary, board);

                    if (isTurnKept == false)
                    {
                        PrintForNewComputerTurn(board, i_ValueDictionary);
                        player1Turn = false;
                    }
                    else
                    {
                        ClearAndPrint(board, i_ValueDictionary);
                        System.Console.WriteLine("You found a pair!, you get the next turn.");
                        i_Player1.Score++;
                    }
                }
                else
                {
                    ClearAndPrint(board, i_ValueDictionary);
                    System.Console.WriteLine("Its computer's turn.");
                    computer.ZeroAndUpdateRevealedTiles(board);
                    bool chose2Tiles = computer.ChooseFirstTile(board);

                    if (chose2Tiles == false)
                    {
                        player1Turn = ComputerGuessTwoTiles(i_ValueDictionary, board, computer);
                    }
                    else
                    {
                        ClearPrintMsgTimeOut(board, i_ValueDictionary, "Computer found a pair!, he gets the next turn.");
                    }
                }
            }

            ClearAndPrint(board, i_ValueDictionary);
            EndGameComputerVSPlayer(computer, i_Player1);

            if (AnotherGame() == true)
            {
                i_Player1.Score = 0;
                computer.Score = 0;
                GetSizesInitAndPlay(i_ValueDictionary, i_Player1, player2, gameStyle);
            }
        }

        private static void ClearPrintMsg(GameBoard i_Board, Dictionary<int, char> i_ValueDictionary, string i_MsgToUser)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            PrintToUser(i_Board, i_ValueDictionary);
            System.Console.WriteLine(i_MsgToUser);
        }

        private static bool PlayerTurn(Dictionary<int, char> io_ValueDictionary, GameBoard io_Board)
        {
            int  openedTile;
            bool isTurnKept;

            GetUserTile(io_Board, out int row, out int col);
            openedTile = io_Board.FirstChoice(row, col);
            ClearAndPrint(io_Board, io_ValueDictionary);
            System.Console.Write("For second tile, ");
            GetUserTile(io_Board, out row, out col);
            isTurnKept = io_Board.SecondChoice(row, col, openedTile);

            return isTurnKept;
        }

        private static bool ComputerGuessTwoTiles(Dictionary<int, char> io_ValueDictionary, GameBoard io_Board, Computer io_Computer)
        {
            bool playerTurn = false;

            ClearPrintMsgTimeOut(io_Board, io_ValueDictionary, "Computer's first tile open.");
            io_Computer.ZeroAndUpdateRevealedTiles(io_Board);
            io_Computer.ChooseSecondTile(io_Board);
            ClearPrintMsgTimeOut(io_Board, io_ValueDictionary, "Computer's second tile open.");
            bool ComputerScore = io_Board.CheckIfScore();

            if (ComputerScore == true)
            {
                ClearPrintMsgTimeOut(io_Board, io_ValueDictionary, "Computer found a pair!, he gets the next turn.");
                io_Board.MakeOpen();
                io_Computer.Score++;
            }
            else
            {
                UnSuccessfulComputerTurn(io_Board, io_ValueDictionary);
                playerTurn = true;
            }

            return playerTurn;
        }

        private static void EndGameComputerVSPlayer(Computer i_Computer, Player i_Player)
        {
            string msgToConsole = string.Empty;

            if (i_Computer.Score > i_Player.Score)
            {
                msgToConsole = string.Format("Computer won the game!!\nDont worry {0} maybe next time..", i_Player.Name);
            }
            else if (i_Player.Score > i_Computer.Score)
            {
                msgToConsole = string.Format("{0}, Amazing work you won agianst the computer!!", i_Player.Name);
            }
            else
            {
                msgToConsole = string.Format("Its a tie!, maybe next time you will win..");
            }

            System.Console.WriteLine(msgToConsole);
            msgToConsole = string.Format("Scores:\n {0}: {1} \n Computer: {2}\n", i_Player.Name, i_Player.Score, i_Computer.Score);
            System.Console.WriteLine(msgToConsole);
        }

        private static void EndGamePlayerVSPlayer(Player i_Player1, Player i_Player2)
        {
            string msgToConsole = string.Empty;

            if (i_Player2.Score > i_Player1.Score)
            {
                msgToConsole = string.Format("{0} won the game!!\nDont worry {1} maybe next time..", i_Player2.Name, i_Player1.Name);
            }
            else if (i_Player1.Score > i_Player2.Score)
            {
                msgToConsole = string.Format("{0} won the game!!\nDont worry {1} maybe next time..", i_Player1.Name, i_Player2.Name);
            }
            else
            {
                msgToConsole = string.Format("Its a tie!");
            }

            System.Console.WriteLine(msgToConsole);
            msgToConsole = string.Format("Scores:\n {0}: {1} \n {2}: {3}\n", i_Player1.Name, i_Player1.Score, i_Player2.Name, i_Player2.Score);
            System.Console.WriteLine(msgToConsole);
        }

        private static void PrintForNewComputerTurn(GameBoard i_Board, Dictionary<int, char> i_ValueDictionary)
        {
            ClearAndPrint(i_Board, i_ValueDictionary);
            TimeOut();
            i_Board.MakeRevealed();
            ClearAndPrint(i_Board, i_ValueDictionary);
        }

        private static void UnSuccessfulComputerTurn(GameBoard i_Board, Dictionary<int, char> i_ValueDictionary)
        {
            TimeOut();
            i_Board.MakeRevealed();
            ClearAndPrint(i_Board, i_ValueDictionary);
        }

        private static void ClearAndPrint(GameBoard i_Board, Dictionary<int, char> i_ValueDictionary)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            PrintToUser(i_Board, i_ValueDictionary);
        }

        private static void ClearPrintMsgTimeOut(GameBoard i_Board, Dictionary<int, char> i_ValueDictionary, string i_MsgToUser)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            PrintToUser(i_Board, i_ValueDictionary);
            System.Console.WriteLine(i_MsgToUser);
            TimeOut();
        }

        private static void PlayGameVSPlayer(Dictionary<int, char> i_ValueDictionary, Player i_Player1, Player i_Player2, int i_Rows, int i_Cols)
        {
            const int   k_NumOfPlayers = 2;
            Player[]    players = new Player[k_NumOfPlayers] { i_Player1, i_Player2 };
            GameBoard   board = new GameBoard(i_Rows, i_Cols);
            bool        isTurnKept = false;
            int         rowInput = 0;
            int         colInput = 0;
            string      msgToUser = string.Empty;
            ePlayerTurn turn = ePlayerTurn.Player1Turn;
            eGameStyle  gameStyle = eGameStyle.PlayerVSPlayer;
            int         openedTile;

            while (board.IsGameWon() == false)
            {
                msgToUser = string.Format("{0} has the turn.\n", players[(int)turn].Name);
                ClearAndPrint(board, i_ValueDictionary);
                System.Console.Write(msgToUser + "For first tile, ");
                GetUserTile(board, out rowInput, out colInput);
                openedTile = board.FirstChoice(rowInput, colInput);
                ClearAndPrint(board, i_ValueDictionary);
                System.Console.Write("For second tile, ");
                GetUserTile(board, out rowInput, out colInput);
                isTurnKept = board.SecondChoice(rowInput, colInput, openedTile);

                if (isTurnKept == false)
                {
                    ClearAndPrint(board, i_ValueDictionary);
                    TimeOut();
                    board.MakeRevealed();
                    ClearAndPrint(board, i_ValueDictionary);
                    turn = ChangeTurns(turn);
                }
                else
                {
                    ClearAndPrint(board, i_ValueDictionary);
                    players[(int)turn].Score++;
                }
            }

            EndGamePlayerVSPlayer(i_Player1, i_Player2);

            if (AnotherGame() == true)
            {
                i_Player1.Score = 0;
                i_Player2.Score = 0;
                GetSizesInitAndPlay(i_ValueDictionary, i_Player1, i_Player2, gameStyle);
            }
        }

        private static void InitDictionary(Dictionary<int, char> o_Dict, int i_Rows, int i_Cols)
        {
            char startingChar = 'Z';
            int  sizeOfDictionary = i_Rows * i_Cols / 2;

            for (int i = 0; i < sizeOfDictionary; i++)
            {
                o_Dict.Add(i, startingChar--);
            }
        }

        private static void TimeOut()
        {
            const int numberOfSecsToTimeOut = 2;
            TimeSpan  interval = new TimeSpan(0, 0, numberOfSecsToTimeOut);

            System.Threading.Thread.Sleep(interval);
        }

        private static void GetSizes(ref int o_Cols, ref int o_Rows)
        {
            const int k_MaxBoardSize = 6;
            const int k_MinBoardSize = 4;
            bool      validRow;
            bool      validCol;

            System.Console.WriteLine("Please enter the size of the board, first number of rows and than number of columns");
            validRow = int.TryParse(System.Console.ReadLine(), out int rows);
            validCol = int.TryParse(System.Console.ReadLine(), out int cols);

            while (validRow == false || (rows < k_MinBoardSize || rows > k_MaxBoardSize) ||
                validCol == false || (cols < k_MinBoardSize || cols > k_MaxBoardSize) || (rows * cols) % 2 != 0)
            {
                if (validRow == false || (rows < k_MinBoardSize || rows > k_MaxBoardSize))
                {
                    System.Console.WriteLine("You have entered invalid number of rows, please try again (choose 4, 5 or 6):");
                    validRow = int.TryParse(System.Console.ReadLine(), out rows);
                }
                else if (validCol == false || (cols < k_MinBoardSize || cols > k_MaxBoardSize))
                {
                    System.Console.WriteLine("You have entered invalid number of columns, please try again (choose 4, 5 or 6):");
                    validCol = int.TryParse(System.Console.ReadLine(), out cols);
                }
                else
                {
                    System.Console.WriteLine("You have entered invalid board size (the number of tiles must be even), please try again:");
                    validRow = int.TryParse(System.Console.ReadLine(), out rows);
                    validCol = int.TryParse(System.Console.ReadLine(), out cols);
                }
            }

            o_Cols = cols;
            o_Rows = rows;
        }

        private static bool BoardIsOdd(int i_Row, int i_Cols)
        {
            return (i_Row * i_Cols) % 2 == 1; 
        }

        private static void GetUserTile(GameBoard i_Board, out int o_Row, out int o_Col)
        {
            string       userInput;
            const string k_ExitGame = "Q";
            const int    k_InputLength = 2;
            const int    k_RowPlace = 1;
            const int    k_ColPlace = 0;
            const int    k_ExitCode = 1;
            int          maxCol = i_Board.r_Cols + 'A';
            int          maxRow = i_Board.r_Rows;
            bool         validInput = false;
            int          chosenCol;
            int          chosenRow;
            o_Col = 0;
            o_Row = 0;

            System.Console.WriteLine("please enter tile location (for example 'E3'):");
            userInput = System.Console.ReadLine();
            while (validInput == false)
            {
                while ((userInput.Length != k_InputLength) || (userInput[k_ColPlace] < 'A' || userInput[k_ColPlace] >= maxCol)
                          || (userInput[k_RowPlace] <= '0' || userInput[k_RowPlace] > maxRow + '0'))
                {
                    if (userInput == k_ExitGame)
                    {
                        System.Environment.Exit(k_ExitCode);
                    }
                    else if (userInput.Length != k_InputLength)
                    {
                        System.Console.WriteLine("You entered invalid length input, please try again:");
                    }
                    else if (userInput[k_ColPlace] < 'A' || userInput[k_ColPlace] >= maxCol)
                    {
                        System.Console.WriteLine("You entered invalid column input, please try again:");
                    }
                    else
                    {
                        System.Console.WriteLine("You entered invalid row input, please try again:");
                    }

                    userInput = System.Console.ReadLine();
                }

                chosenCol = userInput[k_ColPlace] - 'A';
                chosenRow = int.Parse(userInput[k_RowPlace].ToString());
                chosenRow--;

                if (i_Board.m_State[chosenRow, chosenCol] == eCellState.Closed || i_Board.m_State[chosenRow, chosenCol] == eCellState.Revealed)
                {
                    o_Col = chosenCol;
                    o_Row = chosenRow;
                    validInput = true;
                }
                else
                {
                    System.Console.WriteLine("You entered an invalid cell (it is already open), please try again:");
                    userInput = System.Console.ReadLine();
                }
            }
        }

        private static void PrintToUser(GameBoard i_Board, Dictionary<int, char> i_ValueDictionary)
        {
            int  rows = i_Board.r_Rows;
            int  cols = i_Board.r_Cols;
            char startCharCount = 'A';
            int  startIntCount = 1;
            int  key;

            System.Console.Write("    ");

            for (int i = 0; i < cols; i++)
            {
                System.Console.Write(startCharCount);
                System.Console.Write("   ");
                startCharCount++;
            }

            System.Console.WriteLine(string.Empty);

            for (int i = 0; i < rows; i++)
            {
                PrintBetweenLines(cols);
                System.Console.Write(startIntCount + " ");
                startIntCount++;

                for (int j = 0; j < cols; j++)
                {
                    System.Console.Write("| ");

                    if (i_Board.m_State[i, j] == eCellState.Open || i_Board.m_State[i, j] == eCellState.TurnOpen)
                    {
                        key = i_Board.m_Board[i, j];
                        System.Console.Write(i_ValueDictionary[key] + " ");
                    }
                    else
                    {
                        System.Console.Write("  ");
                    }
                }

                System.Console.WriteLine("|");
            }

            PrintBetweenLines(cols);
        }

        private static void PrintBetweenLines(int i_Cols)
        {
            System.Console.Write("  ");
            System.Console.Write("=");

            for (int i = 0; i < i_Cols; i++)
            {
                System.Console.Write("====");
            }

            System.Console.WriteLine(string.Empty);
        }

        private static ePlayerTurn ChangeTurns(ePlayerTurn i_Turn)
        {
            ePlayerTurn changedTurn;

            if (i_Turn == ePlayerTurn.Player1Turn)
            {
                changedTurn = ePlayerTurn.Player2Turn;
            }
            else
            {
                changedTurn = ePlayerTurn.Player1Turn;
            }

            return changedTurn;
        }

        enum eGameStyle
        {
            PlayerVSComputer = 1,
            PlayerVSPlayer
        }

        internal enum ePlayerTurn
        {
            Player1Turn,
            Player2Turn
        }
    }
}