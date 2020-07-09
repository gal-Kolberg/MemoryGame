using System.Windows.Forms;

namespace MemoryGameUI
{
    public class MemoryGameButton : Button
    {
        private readonly int r_Row;
        private readonly int r_Col;

        public MemoryGameButton(int i_Row, int i_Col)
        {
            r_Row = i_Row;
            r_Col = i_Col;
        }

        public int Row
        {
            get
            {
                return r_Row;
            }
        }

        public int Col
        {
            get
            {
                return r_Col;
            }
        }
    }
}
