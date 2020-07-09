using System;
using System.Drawing;
using System.Windows.Forms;
using MemoryGameLogic;

namespace MemoryGameUI
{
    public partial class MemoryGame : Form
    {
        private readonly GameBoard r_MemoryBoard;
        private readonly bool r_IsPvP;
        private readonly int r_Player1NameLength;
        private readonly int r_Player2NameLength;
        private readonly ComputerPlayer r_ComputerPlayer = null;
        private int m_Rows;
        private int m_Cols;
        private bool m_IsFirstTile = true;
        private bool m_Turn = false;
        private int m_ScorePlayer1 = 0;
        private int m_ScorePlayer2 = 0;
        private MemoryGameButton m_FirstChoiceButton = null;
        private MemoryGameButton m_SecondChoiceButton = null;
        private MemoryGameButton[,] m_TileButtonMatrix;
        private eComputerTurnState m_ComputerTurnState;

        public MemoryGame(Point i_GameSize, string i_FirstName, string i_SecondName, bool i_PvP)
        {
            InitializeComponent();
            Rows = i_GameSize.X;
            Cols = i_GameSize.Y;
            ComputerTurnState = eComputerTurnState.FirstTileToChoose;
            r_MemoryBoard = new GameBoard(Rows, Cols);
            r_IsPvP = i_PvP;
            r_ComputerPlayer = r_IsPvP ? new ComputerPlayer(Rows, Cols) : null; 
            r_Player1NameLength = i_FirstName.Length;
            Player1Name.Text = string.Format(i_FirstName + ": 0 Pair(s)");
            r_Player2NameLength = i_SecondName.Length;
            Player2Name.Text = Player2Name.Text = string.Format(i_SecondName + ": 0 Pair(s)");
            SetCurrentPlayerName();
            initializeTiles();
            Size = new Size(new Point(TileButtonMatrix[Rows - 1, Cols - 1].Right + 50, TileButtonMatrix[Rows - 1, Cols - 1].Bottom + 170));
            Controls.Remove(TileButton);
        }

        private void initializeTiles()
        {
            const int k_LeftSpacing = 20;
            const int k_TopSpacing = 20;
            m_TileButtonMatrix = new MemoryGameButton[Rows, Cols];
            Button baseButton = TileButton;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    TileButtonMatrix[i, j] = new MemoryGameButton(i, j);

                    if (i == 0)
                    {
                        TileButtonMatrix[i, j].Top = baseButton.Top;
                    }
                    else
                    {
                        TileButtonMatrix[i, j].Top = TileButtonMatrix[i - 1, j].Bottom + k_TopSpacing;
                    }

                    if (j == 0)
                    {
                        TileButtonMatrix[i, j].Left = baseButton.Left;
                    }
                    else
                    {
                        TileButtonMatrix[i, j].Left = TileButtonMatrix[i, j - 1].Right + k_LeftSpacing;
                    }

                    TileButtonMatrix[i, j].Enabled = true;
                    TileButtonMatrix[i, j].Visible = true;
                    TileButtonMatrix[i, j].Size = baseButton.Size;
                    Controls.Add(TileButtonMatrix[i, j]);
                    TileButtonMatrix[i, j].Click += MemoryGameButton_Click;
                }
            }
        }

        private void MemoryGameButton_Click(object i_Sender, EventArgs i_E)
        {
            MemoryGameButton button = i_Sender as MemoryGameButton;
            int key = MemoryBoard.GetKey(button.Row, button.Col);
            PictureBox picture = GameManager.sr_ValueDictionary[key];

            if (!MemoryBoard.IsOpenOrTurnOpen(button.Row, button.Col))
            {
                if (IsFirstTile)
                {
                    playerChooseFirstTile(button);
                }
                else
                {
                    button.Image = picture.Image;
                    button.BackColor = Turn ? Player2Name.BackColor : Player1Name.BackColor;
                    SecondChoiceButton = button;

                    if (MemoryBoard.SecondChoice(button.Row, button.Col))
                    {
                        playerChooseSameTiles(button);
                    }
                    else
                    {
                        playerChooseDifferentTiles(button);
                    }

                    IsFirstTile = !IsFirstTile;
                }
            }
        }
        
        private void playerChooseFirstTile(MemoryGameButton i_BoardTileButton)
        {
            int key = MemoryBoard.GetKey(i_BoardTileButton.Row, i_BoardTileButton.Col);
            PictureBox picture = GameManager.sr_ValueDictionary[key];

            MemoryBoard.FirstChoice(i_BoardTileButton.Row, i_BoardTileButton.Col);
            i_BoardTileButton.Image = picture.Image;
            IsFirstTile = !IsFirstTile;
            FirstChoiceButton = i_BoardTileButton;
            i_BoardTileButton.BackColor = Turn ? Player2Name.BackColor : Player1Name.BackColor;
        }

        private void playerChooseSameTiles(MemoryGameButton i_BoardTileButton)
        {
            MemoryBoard.MakeOpen(i_BoardTileButton.Row, i_BoardTileButton.Col, FirstChoiceButton.Row, FirstChoiceButton.Col);
            incScore();
            updateLabel();
            FirstChoiceButton.Click -= MemoryGameButton_Click;
            SecondChoiceButton.Click -= MemoryGameButton_Click;
            IsGameWon();
        }

        private void playerChooseDifferentTiles(MemoryGameButton i_BoardTileButton)
        {
            bool disableButtons = false;

            enableDisableButtonsPvPTurn(disableButtons);
            PicTimer.Start();
            MemoryBoard.MakeTurnOpen(i_BoardTileButton.Row, i_BoardTileButton.Col);
            MemoryBoard.MakeRevealed(i_BoardTileButton.Row, i_BoardTileButton.Col, FirstChoiceButton.Row, FirstChoiceButton.Col);
            Turn = !Turn;
            SetCurrentPlayerName();
        }

        private void PicTimer_Tick(object i_Sender, EventArgs i_E)
        {
            PicTimer.Stop();

            bool enableButtons = true;

            enableDisableButtonsPvPTurn(enableButtons);
            FirstChoiceButton.Image = null;
            SecondChoiceButton.Image = null;
            FirstChoiceButton.BackColor = BackColor;
            SecondChoiceButton.BackColor = BackColor;

            if (IsPvP == true && Turn == true)
            {
                enableDisableButtonsComputerTurn(!enableButtons);
                ComputerTimer.Start();
            }
        }

        private void makeComputerTurn()
        {
            const bool k_EnableButtons = false;

            enableDisableButtonsComputerTurn(k_EnableButtons);

            if (ComputerTurnState == eComputerTurnState.FirstTileToChoose)
            {
                firstTileChoosenByComputer();
            }
            else if (ComputerTurnState == eComputerTurnState.SecondTileToChoose)
            {
                secondTileChoosenByComputer();
            }
            else if (ComputerTurnState == eComputerTurnState.EndTurn)
            {
                int firstTileValue = MemoryBoard.GetKey(FirstChoiceButton.Row, FirstChoiceButton.Col);
                int secondTileValue = MemoryBoard.GetKey(SecondChoiceButton.Row, SecondChoiceButton.Col);

                if (firstTileValue == secondTileValue)
                {
                    computerChooseSameTiles();
                }
                else
                {
                    computerChooseDifferentTiles();
                }

                ComputerTurnState = eComputerTurnState.FirstTileToChoose;
            }
        }

        private void firstTileChoosenByComputer()
        {
            Point firstOpened = new Point();
            int key;

            ComputerPlayer.ZeroAndUpdateRevealedTiles(MemoryBoard);
            firstOpened = ComputerPlayer.ChooseFirstTile(MemoryBoard);
            key = MemoryBoard.GetKey(firstOpened.X, firstOpened.Y);
            TileButtonMatrix[firstOpened.X, firstOpened.Y].Image = GameManager.sr_ValueDictionary[key].Image;
            FirstChoiceButton = TileButtonMatrix[firstOpened.X, firstOpened.Y];
            FirstChoiceButton.BackColor = Player2Name.BackColor;
            ComputerTurnState = eComputerTurnState.SecondTileToChoose;
        }

        private void secondTileChoosenByComputer()
        {
            Point secondOpened = new Point();
            int key;

            ComputerPlayer.ZeroAndUpdateRevealedTiles(MemoryBoard);
            secondOpened = ComputerPlayer.ChooseSecondTile(MemoryBoard);
            key = MemoryBoard.GetKey(secondOpened.X, secondOpened.Y);
            TileButtonMatrix[secondOpened.X, secondOpened.Y].Image = GameManager.sr_ValueDictionary[key].Image;
            SecondChoiceButton = TileButtonMatrix[secondOpened.X, secondOpened.Y];
            SecondChoiceButton.BackColor = Player2Name.BackColor;
            ComputerTurnState = eComputerTurnState.EndTurn;
        }

        private void computerChooseSameTiles()
        {
            MemoryBoard.MakeOpen(FirstChoiceButton.Row, FirstChoiceButton.Col, SecondChoiceButton.Row, SecondChoiceButton.Col);
            incScore();
            updateLabel();
            IsGameWon();
        }

        private void computerChooseDifferentTiles()
        {
            const bool k_EnableButtons = true;

            ComputerTimer.Stop();
            MemoryBoard.MakeRevealed(FirstChoiceButton.Row, FirstChoiceButton.Col, SecondChoiceButton.Row, SecondChoiceButton.Col);
            TileButtonMatrix[FirstChoiceButton.Row, FirstChoiceButton.Col].Image = null;
            TileButtonMatrix[SecondChoiceButton.Row, SecondChoiceButton.Col].Image = null;
            TileButtonMatrix[FirstChoiceButton.Row, FirstChoiceButton.Col].BackColor = BackColor;
            TileButtonMatrix[SecondChoiceButton.Row, SecondChoiceButton.Col].BackColor = BackColor;
            Turn = !Turn;
            SetCurrentPlayerName();
            enableDisableButtonsComputerTurn(k_EnableButtons);
        }

        private void IsGameWon()
        {
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            string winnerName = string.Empty;
            string loserName = string.Empty;
            string endGameMsg = string.Empty;

            if (MemoryBoard.IsGameWon())
            {
                ComputerTimer.Stop();

                if (ScorePlayer1 > ScorePlayer2)
                {
                    winnerName = Player1Name.Text.Substring(0, Player1NameLength);
                    loserName = Player2Name.Text.Substring(0, Player2NameLength);
                    endGameMsg = string.Format("congratulations {0} you won the game with {1} pairs! {2}{3} with {4} pairs, Don't worry, maybe next time", winnerName, ScorePlayer1, Environment.NewLine, loserName, ScorePlayer2);
                }
                else if (ScorePlayer2 > ScorePlayer1)
                {
                    winnerName = Player2Name.Text.Substring(0, Player2NameLength);
                    loserName = Player1Name.Text.Substring(0, Player1NameLength);
                    endGameMsg = string.Format("Congratulations!! {0} you won the game with {1} pairs! {2}{3} with {4} pairs, Don't worry, maybe next time", winnerName, ScorePlayer2, Environment.NewLine, loserName, ScorePlayer1);
                }
                else
                {
                    endGameMsg = string.Format("Its a tie!! {0}both of you got {1} pairs!", Environment.NewLine, ScorePlayer1);
                }

                endGameMsg = string.Format("{0}{1}Do you want to start another game?", endGameMsg, Environment.NewLine);

                DialogResult result = MessageBox.Show(endGameMsg, "Memory Game", button);

                if (result == DialogResult.Yes) 
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    DialogResult = DialogResult.No;
                }

                Close();
            }
        }

        private void ComputerTimer_Tick(object i_Sender, EventArgs i_E)
        {
            makeComputerTurn();
        }

        private void enableDisableButtonsPvPTurn(bool i_EnableDisable)
        {
            MemoryGameButton button;

            foreach (Control c in Controls)
            {
                button = c as MemoryGameButton;

                if (button != null)
                {
                    if (c != FirstChoiceButton && c != SecondChoiceButton && MemoryBoard.IsOpen(button.Row, button.Col) == false)
                    {
                        c.Enabled = i_EnableDisable;
                    }
                }
            }
        }

        private void enableDisableButtonsComputerTurn(bool i_EnableDisable)
        {
            MemoryGameButton button;

            foreach (Control c in Controls)
            {
                button = c as MemoryGameButton;

                if (button != null)
                {
                    if (i_EnableDisable == true)
                    {
                        if (MemoryBoard.IsOpen(button.Row, button.Col) == false)
                        {
                            button.Click += MemoryGameButton_Click;
                        }
                    }
                    else
                    {
                        if (MemoryBoard.IsOpen(button.Row, button.Col) == false)
                        {
                            button.Click -= MemoryGameButton_Click;
                        }
                    }
                }
            }
        }

        private void SetCurrentPlayerName()
        {
            if (Turn == false)
            {
                CurrentPlayer.Text = "Current Player: " + Player1Name.Text.Substring(0, Player1NameLength);
                CurrentPlayer.BackColor = Player1Name.BackColor;
            }
            else
            {
                CurrentPlayer.Text = "Current Player: " + Player2Name.Text.Substring(0, Player2NameLength);
                CurrentPlayer.BackColor = Player2Name.BackColor;
            }
        }

        private void incScore()
        {
            if (Turn == false)
            {
                ScorePlayer1++;
            }
            else
            {
                ScorePlayer2++;
            }
        }

        private void updateLabel()
        {
            string updatedLabel;

            if (Turn == false)
            {
                if (ScorePlayer1 == 1) 
                {
                    updatedLabel = string.Format(Player1Name.Text.Substring(0, Player1NameLength) + ": {0} Pair(s)", ScorePlayer1);
                }
                else
                {
                    updatedLabel = string.Format(Player1Name.Text.Substring(0, Player1NameLength) + ": {0} Pairs", ScorePlayer1);
                }

                Player1Name.Text = updatedLabel;
            }
            else
            {
                if (ScorePlayer2 == 1)
                {
                    updatedLabel = string.Format(Player2Name.Text.Substring(0, Player2NameLength) + ": {0} Pair(s)", ScorePlayer2);
                }
                else
                {
                    updatedLabel = string.Format(Player2Name.Text.Substring(0, Player2NameLength) + ": {0} Pairs", ScorePlayer2);
                }

                Player2Name.Text = updatedLabel;
            }
        }

        private enum eComputerTurnState
        {
            FirstTileToChoose = 0,
            SecondTileToChoose,
            EndTurn
        }

        private eComputerTurnState ComputerTurnState
        {
            get
            {
                return m_ComputerTurnState;
            }

            set
            {
                m_ComputerTurnState = value;
            }
        }

        public int Player1NameLength
        {
            get
            {
                return r_Player1NameLength;
            }
        }

        public bool IsPvP
        {
            get
            {
                return r_IsPvP;
            }
        }

        public int Player2NameLength
        {
            get
            {
                return r_Player2NameLength;
            }
        }

       public MemoryGameButton[,] TileButtonMatrix
        {
            get
            {
                return m_TileButtonMatrix;
            }
        }

        public ComputerPlayer ComputerPlayer
        {
            get
            {
                return r_ComputerPlayer;
            }
        }

        public GameBoard MemoryBoard
        {
            get
            {
                return r_MemoryBoard;
            }
        }

        public MemoryGameButton FirstChoiceButton
        {
            get
            {
                return m_FirstChoiceButton;
            }

            set
            {
                m_FirstChoiceButton = value;
            }
        }

        public MemoryGameButton SecondChoiceButton
        {
            get
            {
                return m_SecondChoiceButton;
            }

            set
            {
                m_SecondChoiceButton = value;
            }
        }

        public bool Turn
        {
            get
            {
                return m_Turn;
            }

            set
            {
                m_Turn = value;
            }
        }

        public bool IsFirstTile
        {
            get
            {
                return m_IsFirstTile;
            }

            set
            {
                m_IsFirstTile = value;
            }
        }

        public int Rows
        {
            get
            {
                return m_Rows;
            }

            set
            {
                m_Rows = value;
            }
        }

        public int Cols
        {
            get
            {
                return m_Cols;
            }

            set
            {
                m_Cols = value;
            }
        }

        public int ScorePlayer1
        {
            get
            {
                return m_ScorePlayer1;
            }

            set
            {
                m_ScorePlayer1 = value;
            }
        }

        public int ScorePlayer2
        {
            get
            {
                return m_ScorePlayer2;
            }

            set
            {
                m_ScorePlayer2 = value;
            }
        }
    }
}
