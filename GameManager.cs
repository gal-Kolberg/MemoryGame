using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MemoryGameUI
{
    public static class GameManager
    {
        public static readonly Dictionary<int, PictureBox> sr_ValueDictionary = new Dictionary<int, PictureBox>();

        public static void PlayGame()
        {
            MemoryGameSettings gameSettings = new MemoryGameSettings();
            gameSettings.ShowDialog();

            if (gameSettings.DialogResult == DialogResult.OK)
            {
                MemoryGame memoryGame = null;

                do
                {
                    initDicionaty(gameSettings.BoardSize.X, gameSettings.BoardSize.Y);
                    memoryGame = new MemoryGame(gameSettings.BoardSize, gameSettings.Player1Name, gameSettings.Player2Name, gameSettings.IsPvP);
                    memoryGame.ShowDialog();

                } while (memoryGame.DialogResult == DialogResult.OK);
            }
        }

        private static void initDicionaty(int i_Rows, int i_Cols)
        {
            sr_ValueDictionary.Clear();
            int sizeOfDictionary = i_Rows * i_Cols / 2;
            PictureBox pictureBox;

            for (int i = 0; i < sizeOfDictionary; i++) 
            {
                pictureBox = new PictureBox();
                pictureBox.Load("https://picsum.photos/80");
                pictureBox.InitialImage = null;
                pictureBox.Size = new Size(new Point(85, 85));
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Location = new System.Drawing.Point(216, 307);
                pictureBox.Name = "pictureBox1";

                sr_ValueDictionary.Add(i, pictureBox);
            }
        }
    }
}
