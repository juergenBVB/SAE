using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class GameBoard
    {
        private int size;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }
        private List<Ship> ships;

        internal List<Ship> Ships
        {
            get { return ships; }
            set { ships = value; }
        }
        private List<Square> squares;

        internal List<Square> Squares
        {
            get { return squares; }
            set { squares = value; }
        }

        public GameBoard(int size, List<Ship> ships, List<Square> squares)
        {
            this.size = size;
            this.ships = ships;
            this.squares = squares;
        }
    }
}
