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

        // updates the list of all targetable squares
        protected List<Square> GetLegalSquaresOpp()
        {
            List<Square> sqList = new List<Square>();
            foreach (Square sq in board.Squares)
            {
                if (sq.IsLegal)
                {
                    sqList.Add(sq);
                }
            }
            return sqList;
        }


        // target a specific square on the opponent's gameboard
        public Boolean TargetSquare(Square sq)
        {
            // if square isnt a legal target, do nothing and return false
            if (board.Squares.Contains(sq) && sq.IsLegal)
            {
                hitLog.Add(sq);
                board.Squares.Find(x => x == sq).IsLegal = false;

                // if square is actually a shippart, destroy it
                if (sq.IsShipPart())
                {
                    ShipPart sp = (ShipPart)board.Squares.Find(x => x == sq);
                    sp.Destroy();
                    board.Squares[board.Squares.FindIndex(x => x == sq)] = sp;
                }
                return true;
            }
            else
                return false;
        }

        public Boolean ShipFound()
        {
            foreach (Square sq in board.Squares)
            {
                if (!sq.IsHit && sq.IsShipPart())
                {
                    return true;
                }

            }
            return false;
        }
    }
}
