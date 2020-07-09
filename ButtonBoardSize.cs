using System.Windows.Forms;
using System.Drawing;

namespace MemoryGameUI
{
    public class ButtonBoardSize : Button
    {
        private static readonly int sr_NumberOfSizes = 8;
        private readonly string[] r_BoardSizes = new string[sr_NumberOfSizes];
        private int m_currentTextIndex = 0;

        public ButtonBoardSize()
        {
            BoardSize[0] = "4 x 4";
            BoardSize[1] = "4 x 5";
            BoardSize[2] = "4 x 6";
            BoardSize[3] = "5 x 4";
            BoardSize[4] = "5 x 6";
            BoardSize[5] = "6 x 4";
            BoardSize[6] = "6 x 5";
            BoardSize[7] = "6 x 6";
            Text = BoardSize[0];
        }

        public void ChangeSize()
        {
            CurrentTextIndex = (CurrentTextIndex + 1) % NumberOfSizes;
            Text = BoardSize[CurrentTextIndex];
        }

        public string[] BoardSize
        {
            get
            {
                return r_BoardSizes;
            }
        }

        private int NumberOfSizes
        {
            get
            {
                return sr_NumberOfSizes;
            }
        }

        public string CurrentSizeString
        {
            get
            {
                return BoardSize[CurrentTextIndex];
            }
        }

        public Point CurrentSize
        {
            get
            {
                Point point = new Point
                {
                    X = int.Parse(CurrentSizeString[0].ToString()),
                    Y = int.Parse(CurrentSizeString[4].ToString())
                };

                return point;
            }
        }

        public int CurrentTextIndex
        {
            get
            {
                return m_currentTextIndex;
            }

            set
            {
                m_currentTextIndex = value;
            }
        }
    }
}
