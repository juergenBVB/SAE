using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class Square
    {

        private int positionX, positionY;
        private Boolean isLegal = true;
        private Boolean isHit = false;

        public Boolean IsLegal
        {
            get { return isLegal; }
            set { isLegal = value; }
        }

        public int PositionY
        {
            get { return positionY; }
            set { positionY = value; }
        }

        public int PositionX
        {
            get { return positionX; }
            set { positionX = value; }
        }

        public bool IsHit
        {
            get { return isHit; }
            set { isHit = value; }
        }

        public Square(int x, int y)
        {
            this.positionX = x;
            this.positionY = y;
        }

        public Square()
        {

        }

        public override string ToString()
        {
            String hitMiss = this.IsShipPart() ? "Hit" : "No Hit";
            return String.Format("Square on position x: {0} / y: {1} targeted. Result: {2}.", this.PositionX, this.PositionY, hitMiss);
        }

        // returns true if square is actually a shippart
        public Boolean IsShipPart()
        {
            return this.GetType().Equals(typeof(ShipPart));
        }

        public static int operator -(Square sq1, Square sq2)
        {
            return Math.Min(sq1.PositionX - sq2.PositionX, sq1.PositionY - sq2.PositionY);
        }

        public static Boolean operator ==(Square sq1, Square sq2)
        {
            {
                
            }
            if ((sq1.PositionX == sq2.PositionX) && (sq1.PositionY == sq2.PositionY))
                return true;
            else
                return false;
        }

        public static Boolean operator !=(Square sq1, Square sq2)
        {
            if ((sq1.PositionX == sq2.PositionX) && (sq1.PositionY == sq2.PositionY))
                return false;
            else
                return true;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Square))
            {
                Square sq = (Square)obj;
                if ((this.PositionX == sq.PositionX) && (this.PositionY == sq.PositionY))
                    return true;
            }
            return false;
        }
    }
}
