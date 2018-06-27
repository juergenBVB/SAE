using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class Player
    {
        private List<Square> hitLog;
        private List<Ship> ships;

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
        public Player()
        {

        }

        public void PlaceAllShips(List<Ship> ships)
        {
            this.ships = ships;
        }
    }
}
