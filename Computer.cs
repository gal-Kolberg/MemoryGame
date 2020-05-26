namespace MemoryGame
{
    class Computer
    {
        private const int k_MinValue = 0;
        private const int k_MaxValue = 100;
        private const int k_Difficulty = 75;
        private const int k_MaxCountRevealed = 2;
        private int m_Score;
        private int[] m_CountRevealed;
        
        public Computer(int i_Rows, int i_Cols)
        {
            m_CountRevealed = new int[i_Cols * i_Rows / 2];
            ZeroCountRevealed();
            m_Score = 0;
        }

        internal void ZeroCountRevealed()
        {
            for (int i = 0; i < m_CountRevealed.Length; i++)
            {
                m_CountRevealed[i] = 0;
            }
        }

        internal bool ChooseFirstTile(GameBoard i_Board)
        {
            System.Random randGen = new System.Random();
            int           key = int.MaxValue;
            bool          chose2Tiles = false;

            for (int i = 0; i < m_CountRevealed.Length; i++)
            {
                if (m_CountRevealed[i] == k_MaxCountRevealed)
                {
                    key = i;
                    break;
                }
            }

            if (key != int.MaxValue && randGen.Next(k_MinValue, k_MaxValue) < k_Difficulty)
            {
                for (int i = 0; i < i_Board.r_Rows; i++)
                {
                    for (int j = 0; j < i_Board.r_Cols; j++)
                    {
                        if (i_Board.m_Board[i, j] == key)
                        {
                            i_Board.m_State[i, j] = eCellState.Open;
                        }
                    }
                }

                chose2Tiles = true;
                Score++;
            }
            else
            {
                getValidRandomFirstTile(i_Board, out int o_RandRow, out int o_RandCol);
                i_Board.m_State[o_RandRow, o_RandCol] = eCellState.TurnOpen;
            }

            return chose2Tiles;
        }

        private void getValidRandomFirstTile(GameBoard i_Board, out int o_Row, out int o_Col)
        {
            System.Random randGen = new System.Random();
            int           randRow = randGen.Next(k_MinValue, i_Board.r_Rows);
            int           randCol = randGen.Next(k_MinValue, i_Board.r_Cols);

            while (i_Board.m_State[randRow, randCol] == eCellState.Open)
            {
                randRow = randGen.Next(k_MinValue, i_Board.r_Rows);
                randCol = randGen.Next(k_MinValue, i_Board.r_Cols);
            }

            o_Row = randRow;
            o_Col = randCol;
        }

        internal void ChooseSecondTile(GameBoard i_Board)
        {
            System.Random randGen = new System.Random();
            int           firstTile = int.MaxValue;
            bool          foundFirstTile = false;

            for (int i = 0; i < i_Board.r_Rows && foundFirstTile == false; i++)
            {
                for (int j = 0; j < i_Board.r_Cols && foundFirstTile == false; j++)
                {
                    if (i_Board.m_State[i, j] == eCellState.TurnOpen)
                    {
                        firstTile = i_Board.m_Board[i, j];
                        foundFirstTile = true;
                    }
                }
            }

            if (randGen.Next(k_MinValue, k_MaxValue) < k_Difficulty && m_CountRevealed[firstTile] == k_MaxCountRevealed)
            {
                for (int i = 0; i < i_Board.r_Rows; i++)
                {
                    for (int j = 0; j < i_Board.r_Cols; j++)
                    {
                        if (i_Board.m_Board[i, j] == firstTile) 
                        {
                            i_Board.m_State[i, j] = eCellState.TurnOpen;
                        }
                    }
                }

                Score++;
            }
            else
            {
                getValidRandomSecondTile(i_Board, out int o_RandRow, out int o_RandCol);
                i_Board.m_State[o_RandRow, o_RandCol] = eCellState.TurnOpen;
            }
        }

        internal void UpdateCountRevealed(GameBoard i_Board)
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
            System.Random randGen = new System.Random();
            int           randRow = randGen.Next(k_MinValue, i_Board.r_Rows);
            int           randCol = randGen.Next(k_MinValue, i_Board.r_Cols);

            while (i_Board.m_State[randRow, randCol] == eCellState.Open ||
                i_Board.m_State[randRow, randCol] == eCellState.Revealed || i_Board.m_State[randRow, randCol] == eCellState.TurnOpen)
            {
                randRow = randGen.Next(k_MinValue, i_Board.r_Rows);
                randCol = randGen.Next(k_MinValue, i_Board.r_Cols);
            }

            o_Row = randRow;
            o_Col = randCol;
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        internal void ZeroAndUpdateRevealedTiles(GameBoard i_Board)
        {
            ZeroCountRevealed();
            UpdateCountRevealed(i_Board);
        }
    }
}
