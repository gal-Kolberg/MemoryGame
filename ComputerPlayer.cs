using System;
using System.Drawing;

namespace MemoryGameLogic
{
    public class ComputerPlayer
    {
        private const int k_MinValue = 0;
        private const int k_MaxValue = 100;
        private const int k_Difficulty = 75;
        private const int k_MaxCountRevealed = 2;
        private readonly Random r_RandGen = new Random();
        private int[] m_CountRevealed;

        public ComputerPlayer(int i_Rows, int i_Cols)
        {
            m_CountRevealed = new int[i_Cols * i_Rows / 2];
            ZeroCountRevealed();
        }

        public void ZeroCountRevealed()
        {
            for (int i = 0; i < m_CountRevealed.Length; i++)
            {
                m_CountRevealed[i] = 0;
            }
        }

        public Point ChooseFirstTile(GameBoard i_Board)
        {
            getValidRandomFirstTile(i_Board, out int o_RandRow, out int o_RandCol);
            i_Board.m_State[o_RandRow, o_RandCol] = eCellState.TurnOpen;

            return new Point(o_RandRow, o_RandCol);
        }

        private void getValidRandomFirstTile(GameBoard i_Board, out int o_Row, out int o_Col)
        {
            int randRow = RandGen.Next(k_MinValue, i_Board.r_Rows);
            int randCol = RandGen.Next(k_MinValue, i_Board.r_Cols);

            while (i_Board.m_State[randRow, randCol] == eCellState.Open)
            {
                randRow = RandGen.Next(k_MinValue, i_Board.r_Rows);
                randCol = RandGen.Next(k_MinValue, i_Board.r_Cols);
            }

            o_Row = randRow;
            o_Col = randCol;
        }

        public Point ChooseSecondTile(GameBoard i_Board)
        {
            int firstTile = int.MaxValue;
            bool foundFirstTile = false;
            Point firstTileOpened = new Point();
            Point tileOpened = new Point();

            for (int i = 0; i < i_Board.r_Rows && foundFirstTile == false; i++)
            {
                for (int j = 0; j < i_Board.r_Cols && foundFirstTile == false; j++)
                {
                    if (i_Board.m_State[i, j] == eCellState.TurnOpen)
                    {
                        firstTile = i_Board.m_Board[i, j];
                        firstTileOpened.X = i;
                        firstTileOpened.Y = j;
                    }
                }
            }

            if (RandGen.Next(k_MinValue, k_MaxValue) < k_Difficulty && m_CountRevealed[firstTile] == k_MaxCountRevealed)
            {
                for (int i = 0; i < i_Board.r_Rows; i++)
                {
                    for (int j = 0; j < i_Board.r_Cols; j++)
                    {
                        if (i_Board.m_Board[i, j] == firstTile  && (firstTileOpened.X != i || firstTileOpened.Y != j))
                        {
                            i_Board.m_State[i, j] = eCellState.TurnOpen;
                            tileOpened.X = i;
                            tileOpened.Y = j;
                        }
                    }
                }   
            }
            else
            {
                getValidRandomSecondTile(i_Board, out int o_RandRow, out int o_RandCol);
                i_Board.m_State[o_RandRow, o_RandCol] = eCellState.TurnOpen;
                tileOpened.X = o_RandRow;
                tileOpened.Y = o_RandCol;
            }

            return tileOpened;
        }

        public void UpdateCountRevealed(GameBoard i_Board)
        {
            for (int i = 0; i < i_Board.r_Rows; i++)
            {
                for (int j = 0; j < i_Board.r_Cols; j++)
                {
                    if (i_Board.m_State[i, j] == eCellState.Revealed || i_Board.m_State[i, j] == eCellState.TurnOpen)
                    {
                        m_CountRevealed[i_Board.m_Board[i, j]]++;
                    }
                }
            }
        }

        private void getValidRandomSecondTile(GameBoard i_Board, out int o_Row, out int o_Col)
        {
            int randRow = RandGen.Next(k_MinValue, i_Board.r_Rows);
            int randCol = RandGen.Next(k_MinValue, i_Board.r_Cols);

            while (i_Board.m_State[randRow, randCol] == eCellState.Open || i_Board.m_State[randRow, randCol] == eCellState.TurnOpen)
            {
                randRow = RandGen.Next(k_MinValue, i_Board.r_Rows);
                randCol = RandGen.Next(k_MinValue, i_Board.r_Cols);
            }

            o_Row = randRow;
            o_Col = randCol;
        }

        public Random RandGen
        {
            get
            {
                return r_RandGen;
            }
        }

        public void ZeroAndUpdateRevealedTiles(GameBoard i_Board)
        {
            ZeroCountRevealed();
            UpdateCountRevealed(i_Board);
        }
    }
}
