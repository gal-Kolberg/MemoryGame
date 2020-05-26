namespace MemoryGame
{
    public class GameBoard
    {
        internal readonly int r_Rows;
        internal readonly int r_Cols;
        internal int[,] m_Board;
        internal eCellState[,] m_State;

        public GameBoard(int i_Rows, int i_Cols)
        {
            m_Board = new int[i_Rows, i_Cols];
            m_State = new eCellState[i_Rows, i_Cols];
            r_Rows = i_Rows;
            r_Cols = i_Cols;
            InitState();
            initBoard();
            this.shuffleBoard();
        }

        internal void InitState()
        {
            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Cols; j++)
                {
                    m_State[i, j] = eCellState.Closed;
                }
            }
        }

        private void initBoard()
        {
            int initialValue = 0;
            int changeTile = 0;

            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Cols; j++)
                {
                    m_Board[i, j] = initialValue;
                    changeTile++;

                    if (changeTile % 2 == 0)
                    {
                        initialValue++;
                    }
                }
            }
        }

        private void shuffleBoard()
        {
            System.Random randGen = new System.Random();
            int           rows = this.r_Rows;
            int           cols = this.r_Cols;
            int           minValue = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    swapCells(ref this.m_Board[i, j], ref this.m_Board[randGen.Next(minValue, rows), randGen.Next(minValue, cols)]);
                }
            }
        }

        private void swapCells(ref int i_LeftVal, ref int i_RightVal)
        {
            int temp = i_LeftVal;

            i_LeftVal = i_RightVal;
            i_RightVal = temp;
        }

        public int FirstChoice(int i_RowInput, int i_ColInput)
        {
            m_State[i_RowInput, i_ColInput] = eCellState.TurnOpen;
            return m_Board[i_RowInput, i_ColInput];
        }

        public bool SecondChoice(int i_RowInput, int i_ColInput, int i_OpenedTile)
        {
            bool sameValue;
            bool sameTurn = false;

            m_State[i_RowInput, i_ColInput] = eCellState.TurnOpen;
            sameValue = m_Board[i_RowInput, i_ColInput] == i_OpenedTile;

            if (sameValue == true)
            {
                sameTurn = true;
                MakeOpen();
            }

            return sameTurn;
        }

        internal void MakeRevealed()
        {
            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Cols; j++)
                {
                    if (m_State[i, j] == eCellState.TurnOpen)
                    {
                        m_State[i, j] = eCellState.Revealed;
                    }
                }
            }
        }

        internal bool CheckIfScore()
        {
            int       fstTile = int.MinValue;
            int       sndTile = int.MaxValue;
            int       count = 0;
            const int k_FstFound = 0;
            const int k_SndFound = 1;

            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Cols; j++)
                {
                    if (m_State[i, j] == eCellState.TurnOpen && count == k_FstFound)
                    {
                        fstTile = m_Board[i, j];
                        count++;
                    }
                    else if (m_State[i, j] == eCellState.TurnOpen && count == k_SndFound)
                    {
                        sndTile = m_Board[i, j];
                        break;
                    }
                }
            }

            return fstTile == sndTile;
        }

        internal void MakeOpen()
        {
            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Cols; j++)
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
    }

    internal enum eCellState
    {
        Closed,
        TurnOpen,
        Revealed,
        Open
    }
}