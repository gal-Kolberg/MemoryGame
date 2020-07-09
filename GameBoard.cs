using System;
using System.Drawing;

namespace MemoryGameLogic
{
    public class GameBoard
    {
        internal readonly Random r_RandGen = new System.Random();
        internal readonly int r_Rows;
        internal readonly int r_Cols;
        internal int[,] m_Board;
        internal eCellState[,] m_State;

        internal Random RandGen
        {
            get
            {
                return r_RandGen;
            }
        }

        public GameBoard(int i_Rows, int i_Cols)
        {
            Board = new int[i_Rows, i_Cols];
            m_State = new eCellState[i_Rows, i_Cols];
            r_Rows = i_Rows;
            r_Cols = i_Cols;
            InitState();
            initBoard();
            shuffleBoard();
        }

        internal void InitState()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    State[i, j] = eCellState.Closed;
                }
            }
        }

        public int GetKey(int i_Row, int i_Col)
        {
            return Board[i_Row, i_Col];
        }

        private void initBoard()
        {
            int initialValue = 0;
            int changeTile = 0;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Board[i, j] = initialValue;
                    changeTile++;

                    if (changeTile % 2 == 0)
                    {
                        initialValue++;
                    }
                }
            }
        }

        public bool IsOpen(int i_Row, int i_Col)
        {
            bool isOpen = false;

            if(m_State[i_Row, i_Col] == eCellState.Open)
            {
                isOpen = true;
            }

            return isOpen;
        }

        public bool IsOpenOrTurnOpen(int i_Row, int i_Col)
        {
            bool isOpenOrTurnOpen = false;

            if (m_State[i_Row, i_Col] == eCellState.Open || m_State[i_Row, i_Col] == eCellState.TurnOpen)
            {
                isOpenOrTurnOpen = true;
            }

            return isOpenOrTurnOpen;
        }

        private void shuffleBoard()
        {
            int rows = Rows;
            int cols = Cols;
            int minValue = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    swapCells(ref Board[i, j], ref Board[RandGen.Next(minValue, rows), RandGen.Next(minValue, cols)]);
                }
            }
        }

        private void swapCells(ref int io_LeftVal, ref int io_RightVal)
        {
            int temp = io_LeftVal;

            io_LeftVal = io_RightVal;
            io_RightVal = temp;
        }

        public void FirstChoice(int i_RowInput, int i_ColInput)
        {
            m_State[i_RowInput, i_ColInput] = eCellState.TurnOpen;
        }

        private int lastTileOpened()
        {
            int valueOpened = -1;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (m_State[i, j] == eCellState.TurnOpen)
                    {
                        valueOpened = Board[i, j];
                        break;
                    }
                }
            }

            return valueOpened;
        }

        public Point PointLastOpenedTile()
        {
            Point tileCoordinate = new Point();

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (m_State[i, j] == eCellState.TurnOpen)
                    {
                        tileCoordinate.X = i;
                        tileCoordinate.Y = j;
                        break;
                    }
                }
            }

            return tileCoordinate;
        }

        public void MakeTurnOpen(int i_RowInput, int i_ColInput)
        {
            m_State[i_RowInput, i_ColInput] = eCellState.TurnOpen;
        }

        public bool SecondChoice(int i_RowInput, int i_ColInput)
        {
            bool sameValue;
            bool sameTurn = false;
            int lastOpenedValue = lastTileOpened();

            sameValue = Board[i_RowInput, i_ColInput] == lastOpenedValue;

            if (sameValue == true)
            {
                sameTurn = true;
                MakeOpen();
            }

            return sameTurn;
        }

        public void MakeRevealed(int i_RowFirst, int i_ColFirst, int i_RowSecond, int i_ColSecond)
        {
            m_State[i_RowFirst, i_ColFirst] = eCellState.Revealed;
            m_State[i_RowSecond, i_ColSecond] = eCellState.Revealed;
        }

        internal bool CheckIfScore()
        {
            int fstTile = int.MinValue;
            int sndTile = int.MaxValue;
            int count = 0;
            const int k_FstFound = 0;
            const int k_SndFound = 1;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (m_State[i, j] == eCellState.TurnOpen && count == k_FstFound)
                    {
                        fstTile = Board[i, j];
                        count++;
                    }
                    else if (m_State[i, j] == eCellState.TurnOpen && count == k_SndFound)
                    {
                        sndTile = Board[i, j];
                        break;
                    }
                }
            }

            return fstTile == sndTile;
        }

        public void MakeOpen(int i_RowFirst, int i_ColFirst, int i_RowSecond, int i_ColSecond)
        {
            m_State[i_RowFirst, i_ColFirst] = eCellState.Open;
            m_State[i_RowSecond, i_ColSecond] = eCellState.Open;
        }

        public void MakeOpen()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (m_State[i, j] == eCellState.TurnOpen)
                    {
                        m_State[i, j] = eCellState.Open;
                    }
                }
            }
        }

        public bool IsGameWon()
        {
            bool gameWon = true;

            foreach (eCellState state in m_State)
            {
                if (state != eCellState.Open)
                {
                    gameWon = false;
                    break;
                }
            }

            return gameWon;
        }

        internal int[,] Board
        {
            get
            {
                return m_Board;
            }

            set
            {
                m_Board = value;
            }
        }

        internal int Rows
        {
            get
            {
                return r_Rows;
            }
        }

        internal int Cols
        {
            get
            {
                return r_Cols;
            }
        }

        internal eCellState[,] State
        {
            get
            {
                return m_State;
            }
        }
    }
}