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
        private Boolean isHit = false;

        public Boolean IsHit
        {
            get { return isHit; }
            set { isHit = value; }
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

        public Square(int x, int y)
        {
            this.positionX = x;
            this.positionY = y;
        }

        public Boolean CheckHit()
        {
            this.isHit = true;
            return this.GetType().Equals(typeof(ShipPart));
        }
    }
}
