using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class Player
    {
        protected List<Square> hitLog;
        protected List<Square> legalSquares;
        protected List<Ship> ships;
        protected GameBoard board;

        internal List<Ship> Ships
        {
            get { return ships; }
            set { ships = value; }
        }

        internal List<Square> HitLog
        {
            get { return hitLog; }
            set { hitLog = value; }
        }
        public Player(GameBoard board, List<Ship> ships)
        {
            this.board = board;
            this.ships = ships;
        }

        protected void GetLegalSquares()
        {
            List<Square> tempList = new List<Square>();
            foreach (Square sq in board.Squares)
            {
                foreach (Square hit in hitLog)
                {
                    if ((sq - hit) > 1)
                    {
                        tempList.Add(sq);
                    }
                }
            }
            legalSquares = tempList;
        }
    }
}
